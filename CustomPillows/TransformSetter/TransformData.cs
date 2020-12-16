using CustomPillows.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace CustomPillows.TransformSetter
{
    internal struct TransformData
    {
        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 Position;

        [JsonConverter(typeof(QuaternionConverter))]
        public Quaternion Rotation;

        [JsonConverter(typeof(Vector3Converter))]
        public Vector3 Scale;

        public void ApplyToTransform(Transform transform)
        {
            transform.localPosition = Position;
            transform.localRotation = Rotation;
            transform.localScale = Scale;
        }

        public static TransformData FromTransform(Transform transform)
        {
            var data = new TransformData();

            data.Position = transform.localPosition;
            data.Rotation = transform.localRotation;
            data.Scale = transform.localScale;

            return data;
        }
    }
}