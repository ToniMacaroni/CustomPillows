using System;
using System.Collections;
using System.Threading.Tasks;
using CustomPillows.Helpers;
using SiraUtil.Logging;
using SiraUtil.Tools;
using UnityEngine;

namespace CustomPillows.Loaders
{
    internal class PillowPrefabLoader
    {
        private static readonly string _bundlePath = $"Resources.pillows";

        private readonly SiraLog _logger;

        public GameObject PillowPrefab { get; private set; }

        public bool IsLoaded { get; private set; }

        private PillowPrefabLoader(SiraLog logger)
        {
            _logger = logger;
        }

        public async Task LoadAsync()
        {
            if (IsLoaded) return;

            var loader = new EmbeddedAssetBundleLoader<GameObject>(_bundlePath, "_Pillow");
            var loadResult = await loader.LoadAsync();
            if (!loadResult.Success) return;
            PillowPrefab = loadResult.Asset;

            IsLoaded = true;
        }
    }
}