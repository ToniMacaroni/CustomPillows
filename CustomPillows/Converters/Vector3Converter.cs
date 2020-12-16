using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace CustomPillows.Converters
{
    class Vector3Converter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vec = (value as Vector3?).GetValueOrDefault();

            var obj = new Dictionary<string, float>();
            obj.Add("x", vec.x);
            obj.Add("y", vec.y);
            obj.Add("z", vec.z);

            serializer.Serialize(writer, obj);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new Vector3();
            var obj = serializer.Deserialize<Dictionary<string, float>>(reader);

            result.x = obj["x"];
            result.y = obj["y"];
            result.z = obj["z"];

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector3);
        }
    }
}
