using CustomPillows.Configuration;
using CustomPillows.Loaders;
using CustomPillows.TransformSetter;
using SiraUtil.Logging;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace CustomPillows
{
    internal class Blahaj : MonoBehaviour, IPillow
    {
        private SiraLog _logger;
        private PluginConfig _config;

        public Transform CachedTransform { get; private set; }
        public GameObject CachedGameObject { get; private set; }
        public Material CachedMaterial { get; private set; }
        private static int albedoColorId = 0;
        private static int AlbedoColorId => albedoColorId != 0 ? albedoColorId : (albedoColorId = Shader.PropertyToID("_AlbedoColor"));

        private readonly Color defaultColor = Color.HSVToRGB(200.0f/360.0f, 0.8f, 0.65f).ColorWithAlpha(1.0f);
        private void Awake()
        {
            CachedTransform = transform;
            CachedGameObject = gameObject;
            CachedMaterial = GetComponentInChildren<SkinnedMeshRenderer>().materials.LastOrDefault();
        }

        [Inject]
        void Construct(SiraLog logger, PluginConfig config)
        {
            _logger = logger;
            _config = config;
        }
        
        public void Init(PillowParams pillowParams)
        {
            if (pillowParams.Shape != PillowParams.PillowShape.Blahaj)
                throw new System.Exception("Pillow params had invalid shape set for blahaj pillow");
            pillowParams.TransformSetter.SetTransform(CachedTransform);

            if (_config.BlahajRandomColors)
                CachedMaterial.SetColor(AlbedoColorId, Color.HSVToRGB(Random.value, 0.8f, 0.65f));
            else
                CachedMaterial.SetColor(AlbedoColorId, defaultColor);
        }

        internal class Factory : PlaceholderFactory<IPillow> { }
        internal class BlahajFactory : IFactory<IPillow>
        {
            private readonly DiContainer _container;
            private readonly PillowPrefabLoader _prefabLoader;
            private readonly SiraLog _logger;

            private BlahajFactory(DiContainer container, PillowPrefabLoader prefabLoader, SiraLog logger)
            {
                _container = container;
                _prefabLoader = prefabLoader;
                _logger = logger;
            }

            public IPillow Create()
            {
                var go = Instantiate(_prefabLoader.BlahajPrefab);
                return _container.InstantiateComponent<Blahaj>(go);
            }
        }
    }

    internal class Pillow : MonoBehaviour, IPillow
    {
        private static readonly int _textureID = Shader.PropertyToID("_DetailAlbedoMap");

        private SiraLog _logger;

        public Transform CachedTransform { get; private set; }
        public GameObject CachedGameObject { get; private set; }
        public Material Material;

        private void Awake()
        {
            CachedTransform = transform;
            CachedGameObject = gameObject;

            Material = gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material;
        }

        [Inject]
        private void Construct(SiraLog logger)
        {
            _logger = logger;
        }

        public void Init(PillowParams pillowParams)
        {
            if (pillowParams.Shape != PillowParams.PillowShape.Body) 
                throw new System.Exception("Pillow params had invalid shape set for regular pillow");

            SetTexture(pillowParams.Texture);
            pillowParams.TransformSetter.SetTransform(CachedTransform);
        }

        public void SetTexture(Texture2D texture)
        {
            Material.SetTexture(_textureID, texture);
        }

        internal class Factory : PlaceholderFactory<IPillow> { }

        internal class PillowFactory : IFactory<IPillow>
        {
            private readonly DiContainer _container;
            private readonly PillowPrefabLoader _prefabLoader;
            private readonly SiraLog _logger;

            private PillowFactory(DiContainer container, PillowPrefabLoader prefabLoader, SiraLog logger)
            {
                _container = container;
                _prefabLoader = prefabLoader;
                _logger = logger;
            }

            public IPillow Create()
            {
                var go = Instantiate(_prefabLoader.PillowPrefab);
                return _container.InstantiateComponent<Pillow>(go);
            }
        }
    }

    internal struct PillowParams
    {
        public ITransformSetter TransformSetter;
        public Texture2D Texture;
        public PillowShape Shape;

        public enum PillowShape
        {
            None,
            Body,
            Blahaj
        };
    }
}