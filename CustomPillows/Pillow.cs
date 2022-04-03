using CustomPillows.Loaders;
using CustomPillows.TransformSetter;
using SiraUtil.Logging;
using SiraUtil.Tools;
using UnityEngine;
using Zenject;

namespace CustomPillows
{
    internal class Pillow : MonoBehaviour
    {
        private static readonly int _textureID = Shader.PropertyToID("_DetailAlbedoMap");

        private SiraLog _logger;

        public Transform CachedTransform;
        public Material Material;

        private void Awake()
        {
            CachedTransform = transform;

            Material = gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material;
        }

        [Inject]
        private void Construct(SiraLog logger)
        {
            _logger = logger;
        }

        public void Init(PillowParams pillowParams)
        {
            SetTexture(pillowParams.Texture);
            pillowParams.TransformSetter.SetTransform(CachedTransform);
        }

        public void SetTexture(Texture2D texture)
        {
            Material.SetTexture(_textureID, texture);
        }

        internal struct PillowParams
        {
            public ITransformSetter TransformSetter;
            public Texture2D Texture;
        }

        internal class Factory : PlaceholderFactory<Pillow> { }

        internal class CustomFactory : IFactory<Pillow>
        {
            private readonly DiContainer _container;
            private readonly PillowPrefabLoader _prefabLoader;
            private readonly SiraLog _logger;

            private CustomFactory(DiContainer container, PillowPrefabLoader prefabLoader, SiraLog logger)
            {
                _container = container;
                _prefabLoader = prefabLoader;
                _logger = logger;
            }

            public Pillow Create()
            {
                var go = Instantiate(_prefabLoader.PillowPrefab);
                return _container.InstantiateComponent<Pillow>(go);
            }
        }
    }
}