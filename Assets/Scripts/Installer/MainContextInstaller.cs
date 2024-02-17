using DefaultNamespace;
using Zenject;

namespace Installer
{
    public class MainContextInstaller : MonoInstaller<MainContextInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ScoreService>().AsSingle();
            Container.Bind<ScoreModel>().AsSingle();
        }
    }
}