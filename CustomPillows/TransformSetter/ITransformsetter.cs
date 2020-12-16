using System.Collections.Generic;
using UnityEngine;

namespace CustomPillows.TransformSetter
{
    interface ITransformSetter
    {
        void SetTransform(Transform transform);
        IList<TransformData> GetTransformData();
    }
}
