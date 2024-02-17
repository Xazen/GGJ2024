using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace Installer
{
    public class MainContextInstaller : MonoInstaller<MainContextInstaller>
    {
        [SerializeField]
        private TimerService timerService;

        public override void InstallBindings()
        {
            Container.Bind<TimerService>().FromInstance(timerService).AsSingle();

            Container.Bind<ScoreService>().AsSingle();
            Container.Bind<ScoreModel>().AsSingle();
        }
    }
}