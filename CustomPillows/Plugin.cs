using System.Reflection;
using CustomPillows.Configuration;
using CustomPillows.Installers;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace CustomPillows
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    [NoEnableDisable]
    public class Plugin
    {

        public static string Name { get; private set; }

        [Init]
        public void Init(IPALogger logger, Config conf, Zenjector zenjector)
        {
            Name = Assembly.GetExecutingAssembly().GetName().Name;

            zenjector.UseLogger(logger);
            
            zenjector.Install<CustomPillowsAppInstaller>(Location.App, conf.Generated<PluginConfig>());
            zenjector.Install<CustomPillowsMenuInstaller>(Location.Menu);
            zenjector.Install<CustomPillowsGameInstaller>(Location.GameCore);
        }
    }
}
