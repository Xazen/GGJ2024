using System;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Player
{
    public class GamePlayerInput : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput;

        public Action OnAttackInput;
        public Action OnStartInput;
        public Action OnScreamInput;
        public Action<Vector2> OnMovementInput;

        private float _currentHitCooldown;
        private float _currentScreamCooldown;

        private BalancingConfig _balancingConfig;
        private PlayerModel _playerModel;
        private TimerService _timerService;
        private float animationSpeedMultiplier;

        [Inject]
        [UsedImplicitly]
        public void Inject(BalancingConfig balancingConfig, GamePlayerService gamePlayerService, TimerService timerService, GameService gameService)
        {
            _timerService = timerService;
            _balancingConfig = balancingConfig;
            _playerModel = gamePlayerService.GetPlayerModel(playerInput.user.index);
            gameService.OnRestart += OnRestart;
        }

        private void OnRestart()
        {
            animationSpeedMultiplier = 1;
        }

        private void Start()
        {
            playerInput.onActionTriggered += OnAction;
        }

        private void OnDestroy()
        {
            playerInput.onActionTriggered -= OnAction;
        }

        private void OnAction(InputAction.CallbackContext context)
        {
            if (context.action.name == "Movement")
            {
                OnMovement(context.ReadValue<Vector2>());
            }

            if (!context.started)
            {
                return;
            }

            if (context.action.name == "Attack")
            {
                OnAttack();
            }

            if (context.action.name == "Start")
            {
                OnStart();
            }

            if (context.action.name == "Scream")
            {
                OnScream();
            }
        }

        private void OnScream()
        {
            if (IsAttackBlocked())
            {
                return;
            }

            _currentScreamCooldown = _balancingConfig.ScreamCooldown * animationSpeedMultiplier;
            Debug.Log("currentScreamCooldown: " + _currentScreamCooldown);
            OnScreamInput?.Invoke();
        }

        private void OnStart()
        {
            OnStartInput?.Invoke();
        }

        private void Update()
        {
            if (_timerService.GetTime().TotalSeconds < _balancingConfig.LateGameThreshold)
            {
                animationSpeedMultiplier = _balancingConfig.LateGameAnimationMultiplier;
            }
            else if (_timerService.GetTime().TotalSeconds < _balancingConfig.MidGameThreshold)
            {
                animationSpeedMultiplier = _balancingConfig.MidGameAnimationMultiplier;
            }

            _currentHitCooldown -= Time.deltaTime;
            _currentScreamCooldown -= Time.deltaTime;
        }

        public void OnAttack()
        {
            if (IsAttackBlocked())
            {
                return;
            }

            _currentHitCooldown = _balancingConfig.HitCooldown * animationSpeedMultiplier;
            Debug.Log("currentHitCooldown: " + _currentHitCooldown);
            OnAttackInput?.Invoke();
        }

        private bool IsAttackBlocked()
        {
            return _currentHitCooldown > 0 || _playerModel.IsStaggered || _currentScreamCooldown > 0;
        }

        public void OnMovement(Vector2 movement)
        {
            OnMovementInput?.Invoke(movement);
        }
    }
}