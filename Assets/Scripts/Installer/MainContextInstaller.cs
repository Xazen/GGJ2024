using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace Installer
{
    public class MainContextInstaller : MonoInstaller<MainContextInstaller>
    {
        [SerializeField]
        private GameManager gameManager;

        [SerializeField]
        private TimerService timerService;

        [SerializeField]
        private GamePlayerManagerController gamePlayerManagerController;

        public override void InstallBindings()
        {
            Container.Bind<TimerService>().FromInstance(timerService).AsSingle();
            Container.Bind<GamePlayerManagerController>().FromInstance(gamePlayerManagerController).AsSingle();

            Container.Bind<GameService>().AsSingle();

            Container.Bind<ScoreService>().AsSingle();
            Container.Bind<ScoreModel>().AsSingle();

            Container.Bind<GamePlayerService>().AsSingle();
            Container.Bind<GamePlayerModel>().AsSingle();

            Container.Bind<BattlefieldModel>().AsSingle();
            Container.Bind<BattlefieldService>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameManager>().FromInstance(gameManager).AsSingle();

        }
    }
}