using CustomPillows.Configuration.UI;
using CustomPillows.Configuration.UI.Views;
using CustomPillows.Loaders;
using SiraUtil;
using Zenject;

namespace CustomPillows.Installers
{
    internal class CustomPillowsMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Initializer>().AsSingle();

            // UI stuff
            Container.Bind<ImageListView>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<SettingsView>().FromNewComponentAsViewController().AsSingle();

            Container.BindFlowCoordinator<PillowFlowCoordinator>();
            Container.BindInterfacesTo<MenuButtonManager>().AsSingle();

            
        }
    }
}