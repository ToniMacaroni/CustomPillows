using Zenject;

namespace CustomPillows.Installers
{
    internal class CustomPillowsGameInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.InstantiateComponentOnNewGameObject<PillowGameController>();
        }
    }
}