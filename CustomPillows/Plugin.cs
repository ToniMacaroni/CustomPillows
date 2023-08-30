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
    public class Plugin
    {

        public static string Name { get; private set; }

        [Init]
        public void Init(IPALogger logger, Config conf, Zenjector zenjector)
        {
            zenjector.UseLogger(logger);

            Name = Assembly.GetExecutingAssembly().GetName().Name;

            zenjector.Install<CustomPillowsAppInstaller>(Location.App, new object[] { conf.Generated<PluginConfig>() });
            zenjector.Install<CustomPillowsMenuInstaller>(Location.Menu);
            zenjector.Install<CustomPillowsGameInstaller>(Location.GameCore);
        }

        [OnStart]
        public void OnApplicationStart()
        {
        }

        [OnExit]
        public void OnApplicationQuit()
        {
        }
    }
}
