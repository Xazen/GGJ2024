using System.Collections;
using Configs;
using DefaultNamespace;
using DG.Tweening;
using JetBrains.Annotations;
using Player;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.SocialPlatforms;
using Zenject;

public class GamePlayerActorController : MonoBehaviour
{
    private int _attackKnifeAnimHash = Animator.StringToHash("attackknife");
    private int _movementAnimHash = Animator.StringToHash("movement");
    private int _screamAnimHash = Animator.StringToHash("scream");
    private int _hurtAnimHash = Animator.StringToHash("hurt");

    [SerializeField]
    private GameObject stuffingPrefab;

    [SerializeField]
    private Rigidbody rigidBody;

    [SerializeField]
    private GameObject model;

    [SerializeField]
    private GamePlayerHitbox hitBox;

    [SerializeField]
    private ScreamHitbox screamHitbox;

    [SerializeField]
    private GamePlayerInput gamePlayerInput;

    [SerializeField]
    private Animator vfxAnimator;

    private BalancingConfig _balancingConfig;
    private Vector3 _moveVector;

    private PlayerModel _playerModel;
    private InputUser _inputUser;
    private GameService _gameService;
    private DiContainer _diContainer;
    private PlayerModelConfig _playerModelConfig;
    private Animator _animator;
    private BattlefieldService _battlefieldService;
    private TimerService _timerService;

    public int PlayerIndex => _inputUser.index;
    public float moveSpeedMultiplier = 1.0f;
    public float animationSpeedMultiplier = 1.0f;
    public float stuffMultiplier = 1.0f;

    [Inject]
    [UsedImplicitly]
    public void Inject(BalancingConfig balancingConfig, GamePlayerService gamePlayerService,
        BattlefieldService battlefieldService, GameService gameService, DiContainer diContainer,
        PlayerModelConfig playerModelConfig, TimerService timerService)
    {
        _timerService = timerService;
        _battlefieldService = battlefieldService;
        _playerModelConfig = playerModelConfig;
        _diContainer = diContainer;
        _gameService = gameService;
        _balancingConfig = balancingConfig;

        _gameService.OnRestart += OnGameRestart;
        _playerModel = gamePlayerService.GetPlayerModel(_inputUser.index);
        var location = battlefieldService.GetAndRegisterFreeSpawnLocation(_inputUser.index);
        gameObject.transform.position = location;
        var character = diContainer.InstantiatePrefab(_playerModelConfig.PlayerModelByIndex[_inputUser.index], model.transform);
        _animator = character.GetComponent<Animator>();
        LookTowards(new Vector3(0, location.y, 0));
    }

    private void OnGameRestart()
    {
        Debug.Log("Restart Game");
        _moveVector = Vector3.zero;
        moveSpeedMultiplier = 1.0f;
        animationSpeedMultiplier = 1.0f;
        stuffMultiplier = 1.0f;
        var location = _battlefieldService.GetAndRegisterFreeSpawnLocation(_inputUser.index);
        LookTowards(new Vector3(0, location.y, 0));
    }

    private void Start()
    {
        hitBox.OnAttackHit += OnAttackHit;
        screamHitbox.OnStuffHit += OnStuffHit;
        gamePlayerInput.OnAttackInput += OnAttack;
        gamePlayerInput.OnScreamInput += OnScream;
        gamePlayerInput.OnMovementInput += OnMovement;
    }

    private void OnStuffHit(Stuffing stuffing)
    {
        stuffing.PlayerIndex = PlayerIndex;
        var stuffingDirection = GetStuffDirection(stuffing.transform.position, true);
        var distance = Vector3.Distance(transform.position, stuffing.transform.position) * _balancingConfig.ScreamDistanceMultiplier;
        SendOffStuff(stuffing.gameObject, stuffingDirection, _balancingConfig.ScreamFlyMin-distance, _balancingConfig.ScreamFlyMax-distance);
    }

    private void OnDestroy()
    {
        hitBox.OnAttackHit -= OnAttackHit;
        screamHitbox.OnStuffHit -= OnStuffHit;
        gamePlayerInput.OnAttackInput -= OnAttack;
        gamePlayerInput.OnMovementInput -= OnMovement;
    }

    private void OnScream()
    {
        _animator.SetTrigger(_screamAnimHash);
        vfxAnimator.SetTrigger(_screamAnimHash);
        StartCoroutine(Scream());
        GetComponent<PlayerAudio>().PlayScream();
        Debug.Log("Scream");
    }

    private IEnumerator Scream()
    {
        _playerModel.IsScreaming = true;
        _moveVector = Vector3.zero;
        screamHitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(_balancingConfig.ScreamboxDuration);
        screamHitbox.gameObject.SetActive(false);
        yield return new WaitForSeconds((_balancingConfig.ScreamDuration - _balancingConfig.ScreamboxDuration) * animationSpeedMultiplier);
        Debug.Log("Wait for scream: " + (_balancingConfig.ScreamDuration - _balancingConfig.ScreamboxDuration) * animationSpeedMultiplier);
        _playerModel.IsScreaming = false;
    }

    public void OnAttack()
    {
        _animator.SetTrigger(_attackKnifeAnimHash);
        StartCoroutine(Attack());
        Debug.Log("Attack");
    }

    public void OnMovement(Vector2 movement)
    {
        if (_playerModel.IsStaggered || _playerModel.IsAttacking || !_gameService.IsGameRunning() || _playerModel.IsScreaming)
        {
            return;
        }
        _moveVector = new Vector3(movement.x, 0, movement.y);
    }

    private void OnAttackHit(GamePlayerActorController gamePlayerActorController)
    {
        gamePlayerActorController.OnGotHit(this);
    }

    private void OnGotHit(GamePlayerActorController attacker)
    {
        Debug.Log(gameObject.name +  " got Hit by " + attacker.gameObject.name);
        _playerModel.CurrentStaggeredDuration = _balancingConfig.StaggeredDuration * animationSpeedMultiplier;
        _animator.SetTrigger(_hurtAnimHash);

        GetComponent<PlayerAudio>().PlayHurt();
        attacker.GetComponentInChildren<PlayerAudio>().PlayHit();

        _moveVector = Vector3.zero;
        Knockback(attacker);
        LookTowards(attacker.transform.position);
        SpawnStuffing(attacker);
    }

    private void SpawnStuffing(GamePlayerActorController attacker)
    {
        float stuffingCount = Random.Range(_balancingConfig.StuffingCountMin, _balancingConfig.StuffingCountMax) * stuffMultiplier;
        for (int i = 0; i < stuffingCount; i++)
        {
            var stuffingDirection = GetStuffDirection(attacker.transform.position, false);
            var stuffing = CreateStuff(attacker, stuffingDirection);
            SendOffStuff(stuffing, stuffingDirection, _balancingConfig.StuffingFlyDistanceMin, _balancingConfig.StuffingFlyDistanceMax);
        }
    }

    private void SendOffStuff(GameObject stuffing, Vector3 stuffingDirection, float flyMin, float flyMax)
    {
        float stuffingFlyDistance = Random.Range(flyMin, flyMax);
        float stuffingFlyDuration = Random.Range(_balancingConfig.StuffingFlyDurationMin, _balancingConfig.StuffingFlyDurationMax);
        stuffing.transform
            .DOMove(stuffing.transform.position + stuffingDirection * stuffingFlyDistance,
                stuffingFlyDuration).SetEase(Ease.OutQuint);
    }

    private Vector3 GetStuffDirection(Vector3 referenceObject, bool inverse)
    {
        var stuffingDirection = transform.position - referenceObject;
        if (inverse)
        {
            stuffingDirection *= -1;
        }
        stuffingDirection.y = 0;
        stuffingDirection.Normalize();

        float randomRotationAngle = Random.Range(-_balancingConfig.StuffingDirectionVariance, _balancingConfig.StuffingDirectionVariance);
        Quaternion randomYRotation = Quaternion.Euler(0f, randomRotationAngle, 0f);
        stuffingDirection = randomYRotation * stuffingDirection;
        return stuffingDirection;
    }

    private GameObject CreateStuff(GamePlayerActorController attacker, Vector3 stuffingDirection)
    {
        var stuffing = _diContainer.InstantiatePrefab(stuffingPrefab);
        var stuffingComp = stuffing.GetComponent<Stuffing>();
        stuffingComp.InitWithPlayerIndex(PlayerIndex);
        stuffingComp.PlayerIndex = attacker.PlayerIndex;
        stuffing.transform.position = transform.position + stuffingDirection * 1.1f;
        stuffing.transform.localScale = Vector3.one * Random.Range(_balancingConfig.StuffingScaleMin, _balancingConfig.StuffingScaleMax);
        return stuffing;
    }

    private void Knockback(GamePlayerActorController attacker)
    {
        var knockbackDirection = transform.position - attacker.transform.position;
        knockbackDirection.y = 0;
        knockbackDirection.Normalize();

        gameObject.transform
            .DOMove(gameObject.transform.position + knockbackDirection * _balancingConfig.KnockbackStrength,
                _balancingConfig.KnockbackMoveSpeed).SetEase(Ease.OutBack);
    }

    private void LookTowards(Vector3 location)
    {
        var lookDirection = (transform.position - location) * -1;
        LookAtDirection(lookDirection);
    }

    private void Update()
    {
        if (!_gameService.IsGameRunning())
        {
            return;
        }

        if (_timerService.GetTime().TotalSeconds < _balancingConfig.LateGameThreshold)
        {
            Debug.Log("Late Game");
            moveSpeedMultiplier = _balancingConfig.LateGameMoveSpeedMultiplier;
            animationSpeedMultiplier = _balancingConfig.LateGameAnimationMultiplier;
            stuffMultiplier = _balancingConfig.LateGameStuffMultiplier;
        }
        else if (_timerService.GetTime().TotalSeconds < _balancingConfig.MidGameThreshold)
        {
            Debug.Log("Mid Game");
            moveSpeedMultiplier = _balancingConfig.MidGameMoveSpeedMultiplier;
            animationSpeedMultiplier = _balancingConfig.MidGameAnimationMultiplier;
            stuffMultiplier = _balancingConfig.MidGameStuffSpawnMultiplier;
        }

        _playerModel.CurrentStaggeredDuration -= Time.deltaTime;
        Move();
        _animator.SetFloat(_movementAnimHash, _moveVector.magnitude);
    }

    private void Move()
    {
        if (_playerModel.IsStaggered || _playerModel.IsAttacking || !_gameService.IsGameRunning() || _playerModel.IsScreaming)
        {
            return;
        }

        if (_moveVector != Vector3.zero)
        {
            LookAtDirection(_moveVector);
        }

        if (!IsBlocked(_moveVector))
        {
            var newPosition = rigidBody.position + _moveVector * Time.deltaTime * _balancingConfig.PlayerMovementSpeed * moveSpeedMultiplier;
            Debug.Log("moveSpeedMultiplier: " + moveSpeedMultiplier);
            rigidBody.MovePosition(newPosition);
        }

    }

    private void LookAtDirection(Vector3 direction)
    {
        model.transform.rotation = Quaternion.LookRotation(direction);
    }


    private IEnumerator Attack()
    {
        _playerModel.IsAttacking = true;
        _moveVector = Vector3.zero;
        yield return new WaitForSeconds(7f/30f * animationSpeedMultiplier);
        hitBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(_balancingConfig.HitboxDuration * animationSpeedMultiplier);
        hitBox.gameObject.SetActive(false);
        _playerModel.IsAttacking = false;
    }

    public void SetInputUser(InputUser playerInputUser)
    {
        _inputUser = playerInputUser;
    }

    private bool IsBlocked(Vector3 direction)
    {
        return Physics.Raycast(rigidBody.position, direction, out _, _balancingConfig.CollisionThreshold);
    }
}
