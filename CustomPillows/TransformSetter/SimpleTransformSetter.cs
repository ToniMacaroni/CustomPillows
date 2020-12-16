using System.Collections.Generic;
using UnityEngine;

namespace CustomPillows.TransformSetter
{
    internal class SimpleTransformSetter : ITransformSetter
    {
        private TransformData _transformData;

        public SimpleTransformSetter(TransformData transformData)
        {
            _transformData = transformData;
        }

        public void SetTransform(Transform transform)
        {
            _transformData.ApplyToTransform(transform);
        }

        public IList<TransformData> GetTransformData() => new List<TransformData>{_transformData};
    }
}