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
            Container.Bind<PillowPrefabLoader>().AsSingle();
            Container.Bind<PillowImageLoader>().AsSingle();
            Container.Bind<ConstellationLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<PillowSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<Initializer>().AsSingle();

            Container.BindFactory<Pillow, Pillow.Factory>().FromFactory<Pillow.CustomFactory>();

            // UI stuff
            Container.BindViewController<ImageListView>();
            Container.BindViewController<SettingsView>();

            Container.BindFlowCoordinator<PillowFlowCoordinator>();
            Container.BindInterfacesTo<MenuButtonManager>().AsSingle();

            
        }
    }
}