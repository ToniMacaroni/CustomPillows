using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace CustomPillows.Converters
{
    class QuaternionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vec = (value as Quaternion?).GetValueOrDefault();

            var obj = new Dictionary<string, float>();
            obj.Add("x", vec.x);
            obj.Add("y", vec.y);
            obj.Add("z", vec.z);
            obj.Add("w", vec.w);

            serializer.Serialize(writer, obj);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = serializer.Deserialize<Dictionary<string, float>>(reader);

            return new Quaternion(obj["x"], obj["y"], obj["z"], obj["w"]);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Quaternion);
        }
    }
}