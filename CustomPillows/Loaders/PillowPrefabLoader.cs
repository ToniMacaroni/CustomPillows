using System;
using System.Collections;
using CustomPillows.Helpers;
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

        public void Load(Action finishedCallback)
        {
            if (IsLoaded)
            {
                finishedCallback?.Invoke();
                return;
            }

            SharedCoroutineStarter.instance.StartCoroutine(LoadCoroutine(finishedCallback));
        }

        public IEnumerator LoadCoroutine(Action finishedCallback)
        {
            if (!EmbeddedAssetLoader.TryLoadAssetBundle(_bundlePath, out var assetBundleCreateRequest))
            {
                _logger.Warning("Could not create request");
                yield break;
            }

            yield return assetBundleCreateRequest;

            if (!assetBundleCreateRequest.isDone || !assetBundleCreateRequest.assetBundle)
            {
                _logger.Warning("Could not load asset bundle");
                yield break;
            }

            AssetBundleRequest assetBundleRequest = assetBundleCreateRequest.assetBundle.LoadAssetWithSubAssetsAsync<GameObject>("_Pillow");
            yield return assetBundleRequest;

            if (!assetBundleRequest.isDone || assetBundleRequest.asset == null)
            {
                assetBundleCreateRequest.assetBundle.Unload(true);
                _logger.Warning("Could not load asset from asset bundle");
                yield break;
            }

            assetBundleCreateRequest.assetBundle.Unload(false);

            PillowPrefab = (GameObject) assetBundleRequest.asset;
            IsLoaded = true;

            finishedCallback?.Invoke();
        }
    }
}