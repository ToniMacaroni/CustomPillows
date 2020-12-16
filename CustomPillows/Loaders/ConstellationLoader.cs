using System.Collections.Generic;
using System.IO;
using System.Linq;
using IPA.Utilities;

namespace CustomPillows.Loaders
{
    internal class ConstellationLoader
    {
        public Dictionary<string, Constellation> Constellations;

        public bool IsLoaded { get; private set; }

        private readonly DirectoryInfo _constellationDirectory;

        private ConstellationLoader()
        {
            Constellations = new Dictionary<string, Constellation>();
            _constellationDirectory = new DirectoryInfo(Path.Combine(UnityGame.UserDataPath, Plugin.Name, "Constellations"));
            _constellationDirectory.Create();
        }

        public void Load()
        {
            if (IsLoaded) return;

            foreach (var file in _constellationDirectory.EnumerateFiles())
            {
                var name = file.Name;
                Constellations.Add(name, Constellation.FromString(name, File.ReadAllText(file.FullName)));
            }

            IsLoaded = true;
        }

        public bool TryGetConstellation(string name, out Constellation constellation) =>
            Constellations.TryGetValue(name, out constellation);

        public IList<string> GetConstellationNames() => Constellations.Keys.ToList();
    }
}