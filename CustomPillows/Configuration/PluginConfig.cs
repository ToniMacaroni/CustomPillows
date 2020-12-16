using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace CustomPillows.Configuration
{
    internal class PluginConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string SelectedTextures { get; set; } = "Cubes";

        public string SelectedConstellation { get; set; } = "Default";
    }
}