using System.Collections;
using DefaultNamespace;
using DG.Tweening;
using JetBrains.Annotations;
using Player;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using Zenject;

public class GamePlayerController : MonoBehaviour
{
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
    private BattlefieldService battlefieldService;

    [Inject]
    [UsedImplicitly]
    public void Inject(BalancingConfig balancingConfig, ScoreService scoreService, GamePlayerService gamePlayerService,
        BattlefieldService battlefieldService)
    {
        this.battlefieldService = battlefieldService;
        _scoreService = scoreService;
        _balancingConfig = balancingConfig;

        _playerModel = gamePlayerService.GetPlayerModel(_inputUser.index);
        var location = battlefieldService.GetAndRegisterFreeSpawnLocation(_inputUser.index);
        gameObject.transform.position = location;
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
        Debug.Log("Attack");
    }

    public void OnMovement(Vector2 movement)
    {
        _moveVector = new Vector3(movement.x, 0, movement.y);
    }

    private void OnAttackHit(GamePlayerController gamePlayerController)
    {
        _scoreService.AddScore(_inputUser.index, 1);
        gamePlayerController.OnGotHit(this);
    }

    private void OnGotHit(GamePlayerController gamePlayerController)
    {
        Debug.Log(gameObject.name +  " got Hit by " + gamePlayerController.gameObject.name);
        _playerModel.CurrentStaggeredDuration = _balancingConfig.StaggeredDuration;

        var knockbackDirection = transform.position - gamePlayerController.transform.position;
        knockbackDirection.y = 0;
        knockbackDirection.Normalize();
        gameObject.transform
            .DOMove(gameObject.transform.position + knockbackDirection * _balancingConfig.KnockbackStrength,
                _balancingConfig.KnockbackMoveSpeed).SetEase(Ease.OutBack);

        Quaternion newRotation = Quaternion.LookRotation(knockbackDirection * -1);
        model.transform.rotation = newRotation;
    }

    private void Update()
    {
        _playerModel.CurrentStaggeredDuration -= Time.deltaTime;
        Move();
    }

    private void Move()
    {
        if (_playerModel.IsStaggered)
        {
            return;
        }

        gameObject.transform.position += _moveVector * Time.deltaTime * _balancingConfig.PlayerMovementSpeed;

        if (_moveVector != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(_moveVector);
            model.transform.rotation = newRotation;
        }
    }


    private IEnumerator Attack()
    {
        hitBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(_balancingConfig.HitboxDuration);
        hitBox.gameObject.SetActive(false);
    }

    public void SetInputUser(InputUser playerInputUser)
    {
        _inputUser = playerInputUser;
    }
}
