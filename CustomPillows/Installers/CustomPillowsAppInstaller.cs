using CustomPillows.Configuration;
using CustomPillows.Loaders;
using SiraUtil;
using Zenject;
using Logger = IPA.Logging.Logger;

namespace CustomPillows.Installers
{
    internal class CustomPillowsAppInstaller : Installer
    {
        private readonly PluginConfig _config;

        private CustomPillowsAppInstaller(PluginConfig config)
        {
            _config = config;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_config).AsSingle();

            Container.Bind<PillowPrefabLoader>().AsSingle();
            Container.Bind<PillowImageLoader>().AsSingle();
            Container.Bind<ConstellationLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<PillowSpawner>().AsSingle();

            Container.BindFactory<Pillow, Pillow.Factory>().FromFactory<Pillow.CustomFactory>();
        }
    }
}