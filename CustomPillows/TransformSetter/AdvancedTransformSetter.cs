using System.Collections.Generic;
using UnityEngine;

namespace CustomPillows.TransformSetter
{
    internal class AdvancedTransformSetter : ITransformSetter
    {
        private readonly IList<TransformData> _transformDataList;

        public AdvancedTransformSetter(IList<TransformData> transformDataList)
        {
            _transformDataList = transformDataList;
        }

        public AdvancedTransformSetter(Transform transform)
        {
            var list = new List<Transform>();
            AddChildren(transform, list);

            _transformDataList = new List<TransformData>();

            foreach (var t in list)
            {
                _transformDataList.Add(TransformData.FromTransform(t));
            }
        }

        public void SetTransform(Transform transform)
        {
            var transforms = new List<Transform>();
            AddChildren(transform, transforms);

            for (int i = 0; i < transforms.Count; i++)
            {
                _transformDataList[i].ApplyToTransform(transforms[i]);
            }
        }

        private void AddChildren(Transform transform, IList<Transform> data)
        {
            data.Add(transform);

            foreach (var child in transform)
            {
                AddChildren((Transform)child, data);
            }
        }

        public IList<TransformData> GetTransformData() => _transformDataList;
    }
}