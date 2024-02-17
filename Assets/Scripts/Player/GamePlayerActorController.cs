using System.Collections;
using DefaultNamespace;
using DG.Tweening;
using JetBrains.Annotations;
using Player;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using Zenject;

public class GamePlayerActorController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rigidBody;

    [SerializeField]
    private GameObject model;

    [SerializeField]
    private GamePlayerHitbox hitBox;

    [SerializeField]
    private GamePlayerInput gamePlayerInput;

    private BalancingConfig _balancingConfig;
    private Vector3 _moveVector;

    private ScoreService _scoreService;
    private PlayerModel _playerModel;
    private InputUser _inputUser;
    private GameService _gameService;

    [Inject]
    [UsedImplicitly]
    public void Inject(BalancingConfig balancingConfig, ScoreService scoreService, GamePlayerService gamePlayerService,
        BattlefieldService battlefieldService, GameService gameService)
    {
        _gameService = gameService;
        _scoreService = scoreService;
        _balancingConfig = balancingConfig;

        _playerModel = gamePlayerService.GetPlayerModel(_inputUser.index);
        var location = battlefieldService.GetAndRegisterFreeSpawnLocation(_inputUser.index);
        gameObject.transform.position = location;
        LookTowards(Vector3.zero);
    }

    private void Start()
    {
        hitBox.OnAttackHit += OnAttackHit;
        gamePlayerInput.OnAttackInput += OnAttack;
        gamePlayerInput.OnMovementInput += OnMovement;
    }

    private void OnDestroy()
    {
        hitBox.OnAttackHit -= OnAttackHit;
        gamePlayerInput.OnAttackInput -= OnAttack;
        gamePlayerInput.OnMovementInput -= OnMovement;
    }

    public void OnAttack()
    {
        StartCoroutine(Attack());
        GetComponent<PlayerAudio>().PlayAttack();
        Debug.Log("Attack");
    }

    public void OnMovement(Vector2 movement)
    {
        _moveVector = new Vector3(movement.x, 0, movement.y);
    }

    private void OnAttackHit(GamePlayerActorController gamePlayerActorController)
    {
        _scoreService.AddScore(_inputUser.index, 1);
        gamePlayerActorController.OnGotHit(this);
    }

    private void OnGotHit(GamePlayerActorController gamePlayerActorController)
    {
        Debug.Log(gameObject.name +  " got Hit by " + gamePlayerActorController.gameObject.name);
        _playerModel.CurrentStaggeredDuration = _balancingConfig.StaggeredDuration;

        var knockbackDirection = transform.position - gamePlayerActorController.transform.position;
        knockbackDirection.y = 0;
        knockbackDirection.Normalize();
        gameObject.transform
            .DOMove(gameObject.transform.position + knockbackDirection * _balancingConfig.KnockbackStrength,
                _balancingConfig.KnockbackMoveSpeed).SetEase(Ease.OutBack);

        LookTowards(gamePlayerActorController.transform.position);
    }

    private void LookTowards(Vector3 location)
    {
        var lookDirection = (transform.position - location) * -1;
        LookAtDirection(lookDirection);
    }

    private void Update()
    {
        _playerModel.CurrentStaggeredDuration -= Time.deltaTime;
        Move();
    }

    private void Move()
    {
        if (_playerModel.IsStaggered || _playerModel.IsAttacking || !_gameService.IsGameRunning())
        {
            return;
        }

        if (_moveVector != Vector3.zero)
        {
            LookAtDirection(_moveVector);
        }

        if (!IsBlocked(_moveVector))
        {
            var newPosition = rigidBody.position + _moveVector * Time.deltaTime * _balancingConfig.PlayerMovementSpeed;
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
        hitBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(_balancingConfig.HitboxDuration);
        hitBox.gameObject.SetActive(false);
        _playerModel.IsAttacking = false;
    }

    public void SetInputUser(InputUser playerInputUser)
    {
        _inputUser = playerInputUser;
    }

    private bool IsBlocked(Vector3 direction)
    {
        if (Physics.Raycast(rigidBody.position, direction, out _, _balancingConfig.CollisionThreshold))
        {
            return true;
        }

        return false;
    }
}
