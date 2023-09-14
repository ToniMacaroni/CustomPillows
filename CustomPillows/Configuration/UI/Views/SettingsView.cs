using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using CustomPillows.Loaders;
using SiraUtil.Logging;
using Zenject;


namespace CustomPillows.Configuration.UI.Views
{
    [ViewDefinition("CustomPillows.Configuration.UI.Views.SettingsView")]
    [HotReload(RelativePathToLayout = @"SettingsView")]
    internal class SettingsView : BSMLAutomaticViewController
    {
        public event Action<bool> OnModToggled; 
        public event Action<Constellation> OnConstellationChanged;

        private SiraLog _logger;
        private PluginConfig _config;
        private ConstellationLoader _constellationLoader;

        [UIValue("Constellations")] private IList<object> _constellationList;
        [UIValue("SelectedConstellation")]
        private string _selectedConstellation
        {
            get => _config.SelectedConstellation;
            set => _config.SelectedConstellation = value;
        }

        [UIValue("MixInBlahaj")]
        private bool _mixInBlahaj
        {
            get => _config.MixInBlahaj;
            set {
                _config.MixInBlahaj = value;
                OnModToggled?.Invoke(IsModEnabled);
                if (blahajSlider != null) 
                    blahajSlider.interactable = value;
                if (blahajColorToggle != null)
                    blahajColorToggle.interactable = value;
            }
        }

        [UIComponent("blahaj-threshold")] BeatSaberMarkupLanguage.Components.Settings.SliderSetting blahajSlider;

        [UIValue("BlahajThreshold")]
        private float _blahajTrheshold
        {
            get => _config.BlahajThreshold * 100.0f;
            set
            {
                _config.BlahajThreshold = value / 100.0f;
                OnModToggled?.Invoke(IsModEnabled);
            }
        }

        [UIComponent("blahaj-random")] BeatSaberMarkupLanguage.Components.Settings.ToggleSetting blahajColorToggle;

        [UIValue("RandomColors")]
        private bool _blahajRandom
        {
            get => _config.BlahajRandomColors;
            set
            {
                _config.BlahajRandomColors = value;
                OnModToggled?.Invoke(IsModEnabled);
            }
        }

        [Inject]
        private async void Construct(SiraLog logger, PluginConfig config, ConstellationLoader constellationLoader)
        {
            _logger = logger;
            _config = config;
            _constellationLoader = constellationLoader;

            await _constellationLoader.LoadAsync();

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
