using System;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using CustomPillows.Configuration.UI;
using Zenject;

namespace CustomPillows
{
    internal class MenuButtonManager : IInitializable, IDisposable
    {
        private readonly MenuButton _menuButton;
        private readonly MainFlowCoordinator _mainFlowCoordinator;
        private readonly PillowFlowCoordinator _pillowFlowCoordinator;

        private MenuButtonManager(MainFlowCoordinator mainFlowCoordinator, PillowFlowCoordinator pillowFlowCoordinator)
        {
            _mainFlowCoordinator = mainFlowCoordinator;
            _pillowFlowCoordinator = pillowFlowCoordinator;
            _menuButton = new MenuButton("Custom Pillows", "Weebify your play space", ShowPillowFlowCoordinator);
        }

        public void Initialize()
        {
            MenuButtons.instance.RegisterButton(_menuButton);
        }

        public void Dispose()
        {
            if (MenuButtons.IsSingletonAvailable)
            {
                MenuButtons.instance.UnregisterButton(_menuButton);
            }
        }

        private void ShowPillowFlowCoordinator()
        {
            _mainFlowCoordinator.PresentFlowCoordinator(_pillowFlowCoordinator);
        }
    }
}