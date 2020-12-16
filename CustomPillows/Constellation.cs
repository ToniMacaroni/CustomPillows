using System.Collections.Generic;
using Newtonsoft.Json;

namespace CustomPillows
{
    internal class Constellation
    {
        public string Name;
        public List<TransformDataCollection> TransformDataCollections;

        public static Constellation FromString(string name, string text)
        {
            var result = new Constellation();

            result.TransformDataCollections = JsonConvert.DeserializeObject<List<TransformDataCollection>>(text);
            result.Name = name;

            return result;
        }
    }
}