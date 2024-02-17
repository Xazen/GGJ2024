using System.Collections;
using DefaultNamespace;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject model;

    [SerializeField]
    private Hitbox hitBox;

    private BalancingConfig balancingConfig;
    private Vector3 moveVector;

    private float currentHitCooldown;
    private float currentStraggleDuration;
    private ScoreService scoreService;
    private InputUser inputUser;
    private bool isStraggled => currentStraggleDuration > 0;

    [Inject]
    [UsedImplicitly]
    public void Inject(BalancingConfig balancingConfig, ScoreService scoreService)
    {
        this.scoreService = scoreService;
        this.balancingConfig = balancingConfig;
    }

    private void Start()
    {
        hitBox.OnAttackHit += OnAttackHit;
    }

    private void OnAttackHit(PlayerController playerController)
    {
        scoreService.AddScore(inputUser.index, 1);
        playerController.OnGotHit(this);
    }

    private void OnGotHit(PlayerController playerController)
    {
        Debug.Log(gameObject.name +  " got Hit by " + playerController.gameObject.name);
        currentStraggleDuration = balancingConfig.StraggleDuration;

        var knockbackDirection = transform.position - playerController.transform.position;
        knockbackDirection.y = 0;
        knockbackDirection.Normalize();
        gameObject.transform
            .DOMove(gameObject.transform.position + knockbackDirection * balancingConfig.KnockbackStrength,
                balancingConfig.KnockbackMoveSpeed).SetEase(Ease.OutBack);

        Quaternion newRotation = Quaternion.LookRotation(knockbackDirection * -1);
        model.transform.rotation = newRotation;
    }

    public void OnMovement(InputValue value)
    {
        var movement = value.Get<Vector2>();
        moveVector = new Vector3(movement.x, 0, movement.y);

    }

    private void Update()
    {
        currentHitCooldown -= Time.deltaTime;
        currentStraggleDuration -= Time.deltaTime;
        Move();
    }

    private void Move()
    {
        if (isStraggled)
        {
            return;
        }

        gameObject.transform.position += moveVector * Time.deltaTime * balancingConfig.PlayerMovementSpeed;

        if (moveVector != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveVector);
            model.transform.rotation = newRotation;
        }
    }

    public void OnAttack()
    {
        if (currentHitCooldown > 0)
        {
            return;
        }

        currentHitCooldown = balancingConfig.HitCooldown;
        StartCoroutine(Attack());
        Debug.Log("Attack");
    }

    private IEnumerator Attack()
    {
        hitBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(balancingConfig.HitboxDuration);
        hitBox.gameObject.SetActive(false);
    }

    public void SetInputUser(InputUser inputUser)
    {
        this.inputUser = inputUser;
    }
}
