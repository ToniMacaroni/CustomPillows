using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomPillows.Helpers
{
    public static class EmbeddedAssetUtilities
    {
        /// <summary>
        /// Reads an embedded resource
        /// </summary>
        /// <param name="relativePath">path relative to the root namespace</param>
        /// <param name="data">the read data</param>
        /// <returns></returns>
        public static async Task<ReadDataResult> ReadAsync(string relativePath)
        {
            var result = new ReadDataResult();

            var fullPath = $"{Plugin.Name}.{relativePath}";

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullPath);

            if (stream == null) return result;
            
            result.Data = new byte[stream.Length];
            await stream.ReadAsync(result.Data, 0, (int)stream.Length);

            result.Success = true;

            return result;
        }

        public struct ReadDataResult
        {
            public bool Success;
            public byte[] Data;
        }
    }

    public class EmbeddedAssetBundleLoader<T> where T : UnityEngine.Object
    {
        private readonly string _relativePath;
        private readonly string _assetName;

        public EmbeddedAssetBundleLoader(string relativePath, string assetName)
        {
            _relativePath = relativePath;
            _assetName = assetName;
        }

        public async Task<BundleLoadResult> LoadAsync()
        {
            var taskSource = new TaskCompletionSource<BundleLoadResult>();
            var result = new BundleLoadResult();

            var fileData = await EmbeddedAssetUtilities.ReadAsync(_relativePath);
            if (!fileData.Success) return result;

            AssetBundleCreateRequest asetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(fileData.Data);
            asetBundleCreateRequest.completed += delegate
            {
                AssetBundle assetBundle = asetBundleCreateRequest.assetBundle;
                try
                {
                    AssetBundleRequest assetBundleRequest = assetBundle.LoadAssetAsync<T>(_assetName);
                    assetBundleRequest.completed += delegate
                    {
                        T asset = (T) assetBundleRequest.asset;
                        assetBundle.Unload(asset==null);

                        result.Success = true;
                        result.Asset = asset;

                        taskSource.TrySetResult(result);
                    };
                }
                catch
                {
                    taskSource.TrySetResult(result);
                }
            };
            return await taskSource.Task;
        }

        public struct BundleLoadResult
        {
            public bool Success;
            public T Asset;
        }
    }
}
