using System.Collections.Generic;
using CustomPillows.TransformSetter;

namespace CustomPillows
{
    internal class TransformDataCollection
    {
        public IList<TransformData> TransformData;

        public TransformDataCollection(IList<TransformData> transformData)
        {
            TransformData = transformData;
        }
    }
}