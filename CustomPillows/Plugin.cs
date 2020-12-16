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
            Name = Assembly.GetExecutingAssembly().GetName().Name;

            zenjector.OnApp<CustomPillowsAppInstaller>().WithParameters(conf.Generated<PluginConfig>(), logger);
            zenjector.OnMenu<CustomPillowsMenuInstaller>();
            zenjector.OnGame<CustomPillowsGameInstaller>();
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
