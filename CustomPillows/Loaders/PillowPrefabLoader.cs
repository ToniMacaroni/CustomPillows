using System;
using System.Collections;
using System.Threading.Tasks;
using CustomPillows.Helpers;
using SiraUtil.Logging;
using UnityEngine;

namespace CustomPillows.Loaders
{
    internal class PillowPrefabLoader
    {
        private static readonly string _bundlePath = $"Resources.pillows";
        private static readonly string _blahajPath = $"Resources.blahaj";

        private readonly SiraLog _logger;

        public GameObject PillowPrefab { get; private set; }
        public GameObject BlahajPrefab { get; private set; }

        public bool IsLoaded { get; private set; }

        private PillowPrefabLoader(SiraLog logger)
        {
            _logger = logger;
        }

        public async Task LoadAsync()
        {
            if (IsLoaded) return;

            PillowPrefab ??= await LoadAsyncAtPath(_bundlePath, "_Pillow");
            BlahajPrefab ??= await LoadAsyncAtPath(_blahajPath, "_Pillow");

            IsLoaded = PillowPrefab != null && BlahajPrefab != null;
        }

        public async Task<GameObject> LoadAsyncAtPath(string resourcePath, string assetName)
        {
            var loader = new EmbeddedAssetBundleLoader<GameObject>(resourcePath, assetName);
            var loadResult = await loader.LoadAsync();
            if (!loadResult.Success) return null;
            return loadResult.Asset;
        }
    }
}