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

        [Inject]
        [UsedImplicitly]
        public void Inject(BalancingConfig balancingConfig, GamePlayerService gamePlayerService)
        {
            _balancingConfig = balancingConfig;
            _playerModel = gamePlayerService.GetPlayerModel(playerInput.user.index);
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

            _currentScreamCooldown = _balancingConfig.ScreamCooldown;
            OnScreamInput?.Invoke();
        }

        private void OnStart()
        {
            OnStartInput?.Invoke();
        }

        private void Update()
        {
            _currentHitCooldown -= Time.deltaTime;
            _currentScreamCooldown -= Time.deltaTime;
        }

        public void OnAttack()
        {
            if (IsAttackBlocked())
            {
                return;
            }

            _currentHitCooldown = _balancingConfig.HitCooldown;
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