using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using CustomPillows.Configuration;
using CustomPillows.Loaders;
using CustomPillows.TransformSetter;
using Newtonsoft.Json;
using SiraUtil.Logging;
using SiraUtil.Tools;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace CustomPillows
{
    internal class PillowSpawner : IDisposable
    {
        private IList<Texture2D> _texturePool;
        private Constellation _constellation;

        private readonly SiraLog _logger;
        private readonly PluginConfig _config;
        private readonly Pillow.Factory _factory;

        private readonly IList<Pillow> _spawnedPillows;

        public bool CanSpawn { get; set; }

        private PillowSpawner(SiraLog logger, PluginConfig config, Pillow.Factory factory)
        {
            _logger = logger;
            _config = config;
            _factory = factory;

            _spawnedPillows = new List<Pillow>();

            _texturePool = new List<Texture2D>();
        }

        public void SetTexturePool(IList<Texture2D> textures, bool refresh = true)
        {
            _texturePool = textures;
            if(refresh) Refresh();
        }

        public void SetConstellation(Constellation constellation, bool refresh = true)
        {
            _constellation = constellation;
            if(refresh) Refresh(); 
        }

        public void Refresh()
        {
            if (!_config.IsEnabled) return;

            if (_texturePool == null || _texturePool.Count == 0 || _constellation == null) return;

            DespawnAll();

            var shuffledList = GenerateShuffledList(_texturePool, _constellation.TransformDataCollections.Count);

            //foreach pillow
            for (var i = 0; i < _constellation.TransformDataCollections.Count; i++)
            {
                var collection = _constellation.TransformDataCollections[i];
                var options = new Pillow.PillowParams
                {
                    TransformSetter = new AdvancedTransformSetter(collection.TransformData),
                    Texture = shuffledList[i]
                };

                Spawn(options);
            }
        }

        private IList<Texture2D> GenerateShuffledList(IList<Texture2D> input, int length)
        {
            var newArr = new List<Texture2D>(length);
            for (int i = 0; i < length; i++)
            {
                newArr.Add(input[i % input.Count]);
            }
            newArr.ShuffleInPlace();
            return newArr;
        }

        public void MassSpawn(MassSpawnParams spawnParams)
        {
            if (spawnParams.Textures.Count == 0 || spawnParams.Amount == 0) return;

            for (int i = 0; i < spawnParams.Amount; i++)
            {
                var tSetter = new SimpleTransformSetter(new TransformData
                {
                    Position = GetRandomVector3(spawnParams.AreaStart, spawnParams.AreaEnd),
                    Rotation = Quaternion.identity,
                    Scale = Vector3.one
                });

                var options = new Pillow.PillowParams
                {
                    TransformSetter = tSetter,
                    Texture = spawnParams.Textures[Random.Range(0, spawnParams.Textures.Count)]
                };

                Spawn(options);
            }
        }

        private void Spawn(Pillow.PillowParams spawnParams)
        {
            if (!CanSpawn) return;

            var pillow = _factory.Create();

            pillow.Init(spawnParams);

            _spawnedPillows.Add(pillow);
        }

        public void Despawn(int idx)
        {
            if (idx > _spawnedPillows.Count - 1) return;
            var pillow = _spawnedPillows[idx];
            _spawnedPillows.RemoveAt(idx);
            Object.Destroy(pillow.gameObject);
        }

        public void DespawnAll()
        {
            for (int i = 0; i < _spawnedPillows.Count; i++)
            {
                Object.Destroy(_spawnedPillows[i].gameObject);
            }

            _spawnedPillows.Clear();
        }

        public void SaveTransforms(string path)
        {
            var list = new List<TransformDataCollection>();

            foreach (var pillow in _spawnedPillows)
            {
                var data = new AdvancedTransformSetter(pillow.CachedTransform);
                list.Add(new TransformDataCollection(data.GetTransformData()));
            }

            var json = JsonConvert.SerializeObject(list);

            File.WriteAllText(path, json);
        }

        public void SetActive(bool active)
        {
            foreach (var spawnedPillow in _spawnedPillows)
            {
                spawnedPillow.gameObject.SetActive(active);
            }
        }

        private Vector3 GetRandomVector3(Vector3 min, Vector3 max)
        {
            var x = Random.Range(min.x, max.x);
            var y = Random.Range(min.y, max.y);
            var z = Random.Range(min.z, max.z);

            return new Vector3(x, y, z);
        }

        public void Dispose()
        {
            //DespawnAll();
        }

        internal struct MassSpawnParams
        {
            public int Amount;
            public Vector3 AreaStart;
            public Vector3 AreaEnd;
            public IList<Texture2D> Textures;
        }
    }
}