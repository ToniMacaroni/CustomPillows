using System.Collections;
using System.Collections.Generic;
using System.IO;
using CustomPillows.Configuration;
using IPA.Utilities;
using SiraUtil.Tools;
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

        public void Initialize()
        {
            if (!_config.IsEnabled) return;
            SharedCoroutineStarter.instance.StartCoroutine(Load());
        }

        public IEnumerator Load()
        {
            if (IsInitialized) yield break;

            _logger.Info("Initializing");

            if (!_prefabLoader.IsLoaded)
            {
                yield return _prefabLoader.LoadCoroutine(null);
            }

            if (!LoadTextures()) yield break;

            if (!LoadConstellation()) yield break;

            _logger.Info("Mod content loaded");

            _pillowSpawner.CanSpawn = true;
            _pillowSpawner.Refresh();

            _logger.Info("Initialized");

            IsInitialized = true;
        }

        private bool LoadTextures()
        {
            if (string.IsNullOrWhiteSpace(_config.SelectedTextures)) return false;

            var texNames = _config.SelectedTextures.Split(';');
            if (texNames.Length == 0) return false;

            var textures = new List<Texture2D>();

            foreach (var name in texNames)
            {
                _imageLoader.Load(name);
                if (_imageLoader.TryGetImage(name, out var tex))
                {
                    textures.Add(tex);
                }
            }

            _pillowSpawner.SetTexturePool(textures, false);

            return true;
        }

        private bool LoadConstellation()
        {
            var fullPath = Path.Combine(UnityGame.UserDataPath, Plugin.Name, "state");

            if (!File.Exists(fullPath))
            {
                fullPath = Path.Combine(UnityGame.UserDataPath, Plugin.Name, "Constellations", "Default");
            }

            if (!File.Exists(fullPath)) return false;

            var constellation = Constellation.FromString("state", File.ReadAllText(fullPath));

            _pillowSpawner.SetConstellation(constellation, false);

            return true;
        }
    }
}
