using System;
using System.Collections.Generic;
using System.IO;
using CustomPillows.Helpers;
using IPA.Utilities;
using UnityEngine;

namespace CustomPillows.Loaders
{
    internal class PillowImageLoader
    {
        public bool IsLoaded { get; private set; }

        public Dictionary<string, Texture2D> Images;

        private readonly DirectoryInfo _imageDirectory;

        private PillowImageLoader()
        {
            _imageDirectory = new DirectoryInfo(Path.Combine(UnityGame.UserDataPath, Plugin.Name, "Images"));
            _imageDirectory.Create();

            Images = new Dictionary<string, Texture2D>();
        }

        /// <summary>
        /// Load a texture by it's name
        /// </summary>
        /// <param name="name">Name the texture</param>
        /// <param name="skipCheck">Skip file exists check</param>
        public void Load(string name, bool skipCheck = false)
        {
            if (Images.ContainsKey(name)) return;
            var file = _imageDirectory.GetFile(name + ".png");
            if (!skipCheck && !file.Exists) return;

            var tex = new Texture2D(2, 2);

            using var stream = file.OpenRead();
            var data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            tex.LoadImage(data);
            tex.name = name;

            Images.Add(name, tex);
        }

        public void LoadAll()
        {
            foreach (var file in _imageDirectory.EnumerateFiles("*.png"))
            {
                if(string.Equals(file.Name, "template.png", StringComparison.OrdinalIgnoreCase)) continue;

                string name = file.Name.Replace(".png", "");
                Load(name, true);
            }

            IsLoaded = true;
        }

        public bool TryGetImage(string name, out Texture2D tex) => Images.TryGetValue(name, out tex);
    }
}