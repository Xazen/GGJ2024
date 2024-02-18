using Configs;
using UnityEngine;
using Zenject;

namespace Installer
{
    [CreateAssetMenu(fileName = "BalancingConfigInstaller", menuName = "Installers/BalancingConfigInstaller")]
    public class BalancingConfigInstaller : ScriptableObjectInstaller<BalancingConfigInstaller>
    {
       public BalancingConfig BalancingConfig;
       public PlayerModelConfig PlayerModelConfig;

       public override void InstallBindings()
       {
           Container.BindInstance(BalancingConfig).AsSingle();
           Container.BindInstance(PlayerModelConfig).AsSingle();
       }
    }
}