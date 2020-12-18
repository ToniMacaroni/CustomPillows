using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomPillows.Helpers;
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

        public async Task LoadAsync()
        {
            if (IsLoaded) return;

            foreach (var file in _constellationDirectory.EnumerateFiles())
            {
                var name = file.Name;
                var fileContent = await file.ReadFileTextAsync();
                var constellation = Constellation.FromStringAsync(name, fileContent);
                Constellations.Add(name, constellation);
            }

            IsLoaded = true;
        }

        public bool TryGetConstellation(string name, out Constellation constellation) =>
            Constellations.TryGetValue(name, out constellation);

        public IList<string> GetConstellationNames() => Constellations.Keys.ToList();
    }
}