using DefaultNamespace;
using UI;
using UnityEngine;
using Zenject;

namespace Installer
{
    public class MainContextInstaller : MonoInstaller<MainContextInstaller>
    {
        [Header("Game Manager")]
        [SerializeField]
        private GameManager gameManager;

        [SerializeField]
        private TimerService timerService;

        [SerializeField]
        private GamePlayerManagerController gamePlayerManagerController;

        [SerializeField]
        private AudioService audioService;

        [Header("UI")]
        [SerializeField]
        private SetupPanel setupPanel;

        [SerializeField]
        private InGameHUD inGameHUD;

        public override void InstallBindings()
        {
            // Game Manager
            Container.Bind<TimerService>().FromInstance(timerService).AsSingle();
            Container.Bind<GamePlayerManagerController>().FromInstance(gamePlayerManagerController).AsSingle();
            Container.Bind<AudioService>().FromInstance(audioService).AsSingle();

            Container.Bind<GameService>().AsSingle();
            Container.Bind<UIService>().AsSingle();

            Container.Bind<ScoreService>().AsSingle();
            Container.Bind<ScoreModel>().AsSingle();

            Container.Bind<GamePlayerService>().AsSingle();
            Container.Bind<GamePlayerModel>().AsSingle();

            Container.Bind<BattlefieldModel>().AsSingle();
            Container.Bind<BattlefieldService>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameManager>().FromInstance(gameManager).AsSingle();

            // UI
            Container.Bind<SetupPanel>().FromInstance(setupPanel).AsSingle();
            Container.Bind<InGameHUD>().FromInstance(inGameHUD).AsSingle();
        }
    }
}