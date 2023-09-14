using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CustomPillows.Helpers;
using IPA.Utilities;
using IPA.Utilities.Async;
using SiraUtil.Logging;
using UnityEngine;
using UnityEngine.Networking;

namespace CustomPillows.Loaders
{
    internal class PillowImageLoader
    {
        public bool IsLoaded { get; private set; }

        public Dictionary<string, Texture2D> Images;

        private readonly DirectoryInfo _imageDirectory;
        private readonly SiraLog _logger;

        private PillowImageLoader(SiraLog logger)
        {
            _logger = logger;

            _imageDirectory = new DirectoryInfo(Path.Combine(UnityGame.UserDataPath, Plugin.Name, "Images"));
            _imageDirectory.Create();

            Images = new Dictionary<string, Texture2D>();
        }

        /// <summary>
        /// Load a texture by it's name
        /// </summary>
        /// <param name="name">Name the texture</param>
        /// <param name="skipCheck">Skip file exists check</param>
        public async Task LoadAsync(string name, bool skipCheck = false)
        {
            if (Images.ContainsKey(name)) return;

            var file = _imageDirectory.File(name + ".png");
            if (!skipCheck && !file.Exists) return;

            var data = await file.ReadFileDataAsync();
            var tex = CommonExtensions.CreateTexture(data, name);

            Images.Add(name, tex);
        }

        private async Task<Texture2D> LoadTextureAsync(string path)
        {
            var completionSource = new TaskCompletionSource<Texture2D>();

            UnityWebRequest req = UnityWebRequestTexture.GetTexture("file://" + path);

            req.SendWebRequest().completed += delegate
            {

                completionSource.TrySetResult(((DownloadHandlerTexture) req.downloadHandler).texture);
            };

            return await completionSource.Task;
        }

        /// <summary>
        /// Loads all textures from the "Images" loader
        /// </summary>
        /// <returns></returns>
        public async Task LoadAllAsync()
        {
            if (IsLoaded) return;

            foreach (var file in _imageDirectory.EnumerateFiles("*.png"))
            {
                // don't load the template
                if (string.Equals(file.Name, "template.png", StringComparison.OrdinalIgnoreCase)) continue;

                var name = file.Name.Replace(".png", "");
                await LoadAsync(name, true);
            }

            IsLoaded = true;
        }

        public bool TryGetImage(string name, out Texture2D tex) => Images.TryGetValue(name, out tex);
    }
}