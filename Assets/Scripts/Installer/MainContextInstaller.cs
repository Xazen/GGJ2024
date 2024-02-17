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

        [SerializeField]
        private ResultScreen resultScreen;

        public override void InstallBindings()
        {
            // Game Manager
            Container.Bind<TimerService>().FromInstance(timerService).AsSingle();
            Container.Bind<GamePlayerManagerController>().FromInstance(gamePlayerManagerController).AsSingle();
            Container.Bind<AudioService>().FromInstance(audioService).AsSingle();
            Container.Bind<UIService>().AsSingle();

            Container.Bind<GameService>().AsSingle();

            Container.Bind<ScoreService>().AsSingle();
            Container.Bind<ScoreModel>().AsSingle().WhenInjectedInto<ScoreService>();

            Container.Bind<GamePlayerService>().AsSingle();
            Container.Bind<GamePlayerModel>().AsSingle().WhenInjectedInto<GamePlayerService>();

            Container.Bind<BattlefieldModel>().AsSingle().WhenInjectedInto<BattlefieldService>();
            Container.Bind<BattlefieldService>().AsSingle();

            Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();

            // UI
            Container.Bind<SetupPanel>().FromInstance(setupPanel).AsSingle().WhenInjectedInto<GameManager>();
            Container.Bind<InGameHUD>().FromInstance(inGameHUD).AsSingle().WhenInjectedInto<GameManager>();;
            Container.Bind<ResultScreen>().FromInstance(resultScreen).AsSingle().WhenInjectedInto<GameManager>();;
        }
    }
}