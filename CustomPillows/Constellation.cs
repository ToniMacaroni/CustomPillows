using System.Collections.Generic;
using Newtonsoft.Json;

namespace CustomPillows
{
    internal class Constellation
    {
        public string Name;
        public List<TransformDataCollection> TransformDataCollections;

        public static Constellation FromStringAsync(string name, string text)
        {
            var result = new Constellation();
            result.TransformDataCollections = JsonConvert.DeserializeObject<List<TransformDataCollection>>(text);
            result.Name = name;
            return result;
        }
    }
}