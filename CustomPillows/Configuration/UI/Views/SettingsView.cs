using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using CustomPillows.Loaders;
using Zenject;


namespace CustomPillows.Configuration.UI.Views
{
    //[ViewDefinition("CustomPillows.Configuration.UI.Views.SettingsView.bsml")]
    //[HotReload(RelativePathToLayout = @"SettingsView.bsml")]
    internal class SettingsView : /*BSMLAutomaticViewController*/ BSMLResourceViewController
    {
        public override string ResourceName => "CustomPillows.Configuration.UI.Views.SettingsView";

        public event Action<bool> OnModToggled; 
        public event Action<Constellation> OnConstellationChanged;

        private CPLogger _logger;
        private PluginConfig _config;
        private ConstellationLoader _constellationLoader;

        [UIValue("Constellations")] private IList<object> _constellationList;

        [UIValue("SelectedConstellation")]
        private string _selectedConstellation
        {
            get => _config.SelectedConstellation;
            set => _config.SelectedConstellation = value;
        }

        [Inject]
        private void Construct(CPLogger logger, PluginConfig config, ConstellationLoader constellationLoader)
        {
            _logger = logger;
            _config = config;
            _constellationLoader = constellationLoader;

            _constellationLoader.Load();

            FillConstellations();
        }

        private void FillConstellations()
        {
            _constellationList = _constellationLoader.GetConstellationNames().Cast<object>().ToList();
        }

        [UIValue("mod-enabled")]
        private bool IsModEnabled
        {
            get => _config.IsEnabled;
            set
            {
                _config.IsEnabled = value;
                OnModToggled?.Invoke(value);
            }
        }

        [UIAction("OnConstellationSelected")]
        private void OnConstellationSelected(object value)
        {
            if (!_constellationLoader.TryGetConstellation((string) value, out var constellation)) return;
            OnConstellationChanged?.Invoke(constellation);
            _config.SelectedConstellation = (string) value;
        }
    }
}
