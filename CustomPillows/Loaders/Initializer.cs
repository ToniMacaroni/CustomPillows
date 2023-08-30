using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CustomPillows.Configuration;
using CustomPillows.Helpers;
using IPA.Utilities;
using IPA.Utilities.Async;
using SiraUtil.Logging;
using UnityEngine;
using Zenject;
using File = System.IO.File;

namespace CustomPillows.Loaders
{
    class Initializer : IInitializable
    {
        private readonly SiraLog _logger;
        private readonly PluginConfig _config;
        private readonly PillowImageLoader _imageLoader;
        private readonly PillowPrefabLoader _prefabLoader;
        private readonly PillowSpawner _pillowSpawner;

        public bool IsInitialized { get; private set; }

        private Initializer(SiraLog logger, PluginConfig config, PillowImageLoader imageLoader, PillowPrefabLoader prefabLoader, PillowSpawner pillowSpawner)
        {
            _logger = logger;
            _config = config;
            _imageLoader = imageLoader;
            _prefabLoader = prefabLoader;
            _pillowSpawner = pillowSpawner;
        }

        public async void Initialize()
        {
            if (!_config.IsEnabled) return;
            await Load();
        }

        public async Task Load()
        {
            if (IsInitialized) return;

            _logger.Info("Initializing");

            await _prefabLoader.LoadAsync();

            if (!await LoadTextures()) return;

            if (!await LoadConstellation()) return;

            _logger.Info("Mod content loaded");

            _pillowSpawner.CanSpawn = true;
            _pillowSpawner.Refresh();

            _logger.Info("Initialized");

            IsInitialized = true;
        }

        private async Task<bool> LoadTextures()
        {
            if (string.IsNullOrWhiteSpace(_config.SelectedTextures)) return false;

            var texNames = _config.SelectedTextures.Split(';');
            if (texNames.Length == 0) return false;

            var textures = new List<Texture2D>();

            foreach (var name in texNames)
            {
                await _imageLoader.LoadAsync(name);

                if (_imageLoader.TryGetImage(name, out var tex))
                {
                    textures.Add(tex);
                }
            }

            _pillowSpawner.SetTexturePool(textures, false);

            return true;
        }

        private async Task<bool> LoadConstellation()
        {
            // first get the safe file "state"
            var fullPath = Path.Combine(UnityGame.UserDataPath, Plugin.Name, "state");

            // if it doesn't exist, use a constellation
            if (!File.Exists(fullPath))
            {
                fullPath = Path.Combine(UnityGame.UserDataPath, Plugin.Name, "Constellations", "Default");
            }

            // if this also doesn't exist, return
            if (!File.Exists(fullPath)) return false;

            var text = await CommonExtensions.ReadFileTextAsync(fullPath);
            var constellation = Constellation.FromStringAsync("state", text);

            _pillowSpawner.SetConstellation(constellation, false);

            return true;
        }
    }
}
