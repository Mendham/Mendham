using Newtonsoft.Json;
using System;

namespace Mendham.Infrastructure.Http.Test.TestObjects
{
    public class ObjectWithReadOnlyPropertiesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ObjectWithReadOnlyProperties).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<DeserializerHelper>(reader);
            return new ObjectWithReadOnlyProperties(source.Value1, source.Value2);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ObjectWithReadOnlyProperties source = value as ObjectWithReadOnlyProperties;

            var target = new DeserializerHelper
            {
                Value1 = source.Value1,
                Value2 = source.Value2
            };

            serializer.Serialize(writer, target);
        }

        private class DeserializerHelper
        {
            public string Value1 { get; set; }
            public int Value2 { get; set; }
        }
    }
}
