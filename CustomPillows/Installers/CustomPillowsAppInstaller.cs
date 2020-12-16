using CustomPillows.Configuration;
using CustomPillows.Loaders;
using Zenject;
using Logger = IPA.Logging.Logger;

namespace CustomPillows.Installers
{
    internal class CustomPillowsAppInstaller : Installer
    {
        private readonly PluginConfig _config;
        private readonly Logger _logger;

        private CustomPillowsAppInstaller(PluginConfig config, Logger logger)
        {
            _config = config;
            _logger = logger;
        }

        public override void InstallBindings()
        {
            Container.Bind<CPLogger>().AsSingle().WithArguments(_logger);
            Container.BindInstance(_config).AsSingle();

            Container.Bind<PillowPrefabLoader>().AsSingle();
            Container.Bind<PillowImageLoader>().AsSingle();
            Container.Bind<ConstellationLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<PillowSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<Initializer>().AsSingle();

            Container.BindFactory<Pillow, Pillow.Factory>().FromFactory<Pillow.CustomFactory>();
        }
    }
}