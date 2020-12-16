using System.Collections.Generic;
using System.IO;
using System.Linq;
using BeatSaberMarkupLanguage;
using CustomPillows.Configuration.UI.Views;
using CustomPillows.Loaders;
using HMUI;
using IPA.Utilities;
using UnityEngine;
using Zenject;

namespace CustomPillows.Configuration.UI
{
    internal class PillowFlowCoordinator : FlowCoordinator
    {
        private MainFlowCoordinator _mainFlowCoordinator;

        private ImageListView _imageListView;
        private SettingsView _settingsView;
        private PluginConfig _config;
        private PillowSpawner _pillowSpawner;
        private Initializer _initializer;

        [Inject]
        private void Construct(MainFlowCoordinator mainFlowCoordinator, ImageListView imageListView,
            SettingsView settingsView, PluginConfig config, PillowSpawner pillowSpawner, Initializer initializer)
        {
            _mainFlowCoordinator = mainFlowCoordinator;
            _imageListView = imageListView;
            _settingsView = settingsView;
            _config = config;
            _pillowSpawner = pillowSpawner;
            _initializer = initializer;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("Custom Pillows");
                showBackButton = true;
                ProvideInitialViewControllers(_imageListView, null, _settingsView);
            }

            _imageListView.OnSelectedTexturesChanged += OnSelectedTexturesChanged;
            _imageListView.OnSaveRequested += OnSaveRequested;
            _imageListView.OnRefreshRequested += OnRefreshRequested;

            _settingsView.OnConstellationChanged += OnSelectedConstellationChanged;
            _settingsView.OnModToggled += ToggleMod;
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            OnSaveRequested();
            Cleanup();

            base.BackButtonWasPressed(topViewController);
            _mainFlowCoordinator.DismissFlowCoordinator(this);
        }

        private void Cleanup()
        {
            _imageListView.OnSelectedTexturesChanged -= OnSelectedTexturesChanged;
            _imageListView.OnSaveRequested -= OnSaveRequested;
            _imageListView.OnRefreshRequested -= OnRefreshRequested;

            _settingsView.OnConstellationChanged -= OnSelectedConstellationChanged;
            _settingsView.OnModToggled -= ToggleMod;
        }

        private void OnSelectedTexturesChanged(IList<Texture2D> textures)
        {
            _config.SelectedTextures = string.Join(";", textures.Select(tex => tex.name));

            _pillowSpawner.SetTexturePool(textures);
        }

        private void OnSelectedConstellationChanged(Constellation constellation)
        {
            _pillowSpawner.SetConstellation(constellation);
        }

        private void OnSaveRequested()
        {
            if (!_config.IsEnabled) return;

            var filename = Path.Combine(UnityGame.UserDataPath, Plugin.Name, "state");
            _pillowSpawner.SaveTransforms(filename);
        }

        private void OnRefreshRequested()
        {
            _pillowSpawner.Refresh();
        }

        private void ToggleMod(bool modEnabled)
        {
            if (modEnabled)
            {
                _initializer.Initialize();
                _pillowSpawner.Refresh();
            }
            else
            {
                _pillowSpawner.DespawnAll();
            }
        }
    }
}