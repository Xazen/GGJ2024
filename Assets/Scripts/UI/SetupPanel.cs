using DefaultNamespace;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class SetupPanel : UIMonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI infoText;

        [SerializeField]
        private GameObject mainMenu;

        private GameService _gameService;
        private BalancingConfig _balancingConfig;
        private AudioService _audioService;

        [Inject]
        [UsedImplicitly]
        private void Inject(GameService gameService, BalancingConfig balancingConfig, AudioService audioService)
        {
            _audioService = audioService;
            _balancingConfig = balancingConfig;
            _gameService = gameService;
        }

        private void Update()
        {
            if (_gameService.GetPlayerCount() < _balancingConfig.MinPlayerCount)
            {
                infoText.text = "Press any button to join the game!";
            }
            else
            {
                infoText.text = "Press start / enter to start the game!";
            }

            if (Input.anyKeyDown && mainMenu.activeSelf)
            {
                _audioService.EndStartingScreen();
                mainMenu.SetActive(false);
            }
        }
    }
}