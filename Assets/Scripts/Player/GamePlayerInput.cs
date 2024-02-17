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
        public Action<Vector2> OnMovementInput;

        private float _currentHitCooldown;

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

            if (context.action.name == "Attack")
            {
                OnAttack();
            }

            if (context.action.name == "Start")
            {
                OnStart();
            }
        }

        private void OnStart()
        {
            OnStartInput?.Invoke();
        }

        private void Update()
        {
            _currentHitCooldown -= Time.deltaTime;
        }

        public void OnAttack()
        {
            if (_currentHitCooldown > 0 || _playerModel.IsStaggered)
            {
                return;
            }

            _currentHitCooldown = _balancingConfig.HitCooldown;
            OnAttackInput?.Invoke();
        }

        public void OnMovement(Vector2 movement)
        {
            OnMovementInput?.Invoke(movement);
        }
    }
}