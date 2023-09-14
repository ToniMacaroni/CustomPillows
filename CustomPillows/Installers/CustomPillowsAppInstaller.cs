using CustomPillows.Configuration;
using CustomPillows.Loaders;
using SiraUtil.Extras;
using SiraUtil.Logging;
using Zenject;
using Logger = IPA.Logging.Logger;

namespace CustomPillows.Installers
{
    internal class CustomPillowsAppInstaller : Installer
    {
        private readonly PluginConfig _config;
        private readonly Logger _logger;

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

            Container.BindFactory<IPillow, Pillow.Factory>().FromFactory<Pillow.PillowFactory>();
            Container.BindFactory<IPillow, Blahaj.Factory>().FromFactory<Blahaj.BlahajFactory>();
        }
    }
}
