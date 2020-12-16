using CustomPillows.Configuration.UI;
using CustomPillows.Configuration.UI.Views;
using SiraUtil;
using Zenject;

namespace CustomPillows.Installers
{
    internal class CustomPillowsMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindViewController<ImageListView>();
            Container.BindViewController<SettingsView>();

            Container.BindFlowCoordinator<PillowFlowCoordinator>();
            Container.BindInterfacesTo<MenuButtonManager>().AsSingle();
        }
    }
}