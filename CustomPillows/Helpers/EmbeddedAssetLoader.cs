using System.Reflection;
using UnityEngine;

namespace CustomPillows.Helpers
{
    public static class EmbeddedAssetLoader
    {
        public static bool TryLoad(string relativePath, out byte[] data)
        {
            data = null;

            var fullPath = $"{Plugin.Name}.{relativePath}";

            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullPath);

            if (stream == null) return false;
            
            data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            stream.Close();

            return true;
        }

        public static bool TryLoadAssetBundle(string relativePath, out AssetBundleCreateRequest createRequest)
        {
            if (!TryLoad(relativePath, out var data))
            {
                createRequest = null;
                return false;
            }

            createRequest = AssetBundle.LoadFromMemoryAsync(data);
            return true;
        }
    }
}
