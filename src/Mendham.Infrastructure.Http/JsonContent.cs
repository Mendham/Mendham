using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Mendham.Infrastructure.Http
{
    /// <summary>
    /// Provides HTTP content based on a json serialized string.
    /// </summary>
    public class JsonContent : StringContent
    {
        public JsonContent(string serializedContent) : base(serializedContent, Encoding.UTF8, "application/json")
        {
            serializedContent.VerifyArgumentNotNullOrWhiteSpace(nameof(serializedContent), 
                "Serialized content was null or whitespace");
        }

        /// <summary>
        /// Creates <see cref="JsonContent"/> by serializing the object <paramref name="value"/>
        /// </summary>
        /// <param name="value">Object to be serialized</param>
        /// <returns><see cref="JsonContent"/> with the serialized version of <paramref name="value"/></returns>
        public static JsonContent FromObject(object value)
        {
            value.VerifyArgumentNotNull(nameof(value));

            return FromObject(value, new JsonConverter[0]);
        }

        /// <summary>
        /// Creates <see cref="JsonContent"/> by serializing the object <paramref name="value"/> using the provided set of <see cref="JsonConverter"/>
        /// </summary>
        /// <param name="value">Object to be serialized</param>
        /// <param name="converters">Converters to use when serializing value</param>
        /// <returns><see cref="JsonContent"/> with the serialized version of <paramref name="value"/></returns>
        public static JsonContent FromObject(object value, params JsonConverter[] converters)
        {
            value.VerifyArgumentNotNull(nameof(value));

            string serializedObject = JsonConvert.SerializeObject(value, converters);
            return new JsonContent(serializedObject);
        }

        /// <summary>
        /// Creates <see cref="JsonContent"/> by serializing the object <paramref name="value"/> using <see cref="JsonSerializerSettings"/>
        /// </summary>
        /// <param name="value">Object to be serialized</param>
        /// <param name="settings">Settings to use when serializing</param>
        /// <returns><see cref="JsonContent"/> with the serialized version of <paramref name="value"/></returns>
        public static JsonContent FromObject(object value, JsonSerializerSettings settings)
        {
            value.VerifyArgumentNotNull(nameof(value));
            settings.VerifyArgumentNotNull(nameof(settings));

            string serializedObject = JsonConvert.SerializeObject(value, settings);
            return new JsonContent(serializedObject);
        }
    }
}
