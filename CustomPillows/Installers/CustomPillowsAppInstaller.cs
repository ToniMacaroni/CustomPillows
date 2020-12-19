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
        private readonly Logger _logger;

        private CustomPillowsAppInstaller(PluginConfig config, Logger logger)
        {
            _config = config;
            _logger = logger;
        }

        public override void InstallBindings()
        {
            Container.BindLoggerAsSiraLogger(_logger);
            Container.BindInstance(_config).AsSingle();
        }
    }
}