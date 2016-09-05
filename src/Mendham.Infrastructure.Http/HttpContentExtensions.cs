using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.Http
{
    public static class HttpContentExtensions
    {
        /// <summary>
        /// Gets the object from HTTP content with a media type of "application/json"
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <returns>Object based on the HTTP content</returns>
        /// <exception cref="InvalidMediaTypeException"><paramref name="httpContent"/> is not of media type "application/json"</exception>
        public static Task<T> ReadAsJsonAsync<T>(this HttpContent httpContent)
        {
            httpContent.VerifyArgumentNotNull(nameof(httpContent));

            return ReadAsJsonAsync<T>(httpContent, new JsonConverter[0]);
        }

        /// <summary>
        /// Gets the object from HTTP content with a media type of "application/json" using the provided set of <see cref="JsonConverter"/>
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="converters">Converters to use when processing value</param>
        /// <returns>Object based on the HTTP content</returns>
        /// <exception cref="InvalidMediaTypeException"><paramref name="httpContent"/> is not of media type "application/json"</exception>
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent httpContent, params JsonConverter[] converters)
        {
            httpContent.VerifyArgumentNotNull(nameof(httpContent));

            var contentString = await ValidateAndGetString(httpContent);
            return JsonConvert.DeserializeObject<T>(contentString, converters);
        }

        /// <summary>
        /// Gets the object from HTTP content with a media type of "application/json" using <see cref="JsonSerializerSettings"/> 
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="settings">Settings to use when processing</param>
        /// <returns>Object based on the HTTP content</returns>
        /// <exception cref="InvalidMediaTypeException"><paramref name="httpContent"/> is not of media type "application/json"</exception>
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent httpContent, JsonSerializerSettings settings)
        {
            httpContent.VerifyArgumentNotNull(nameof(httpContent));
            settings.VerifyArgumentNotNull(nameof(settings));

            var contentString = await ValidateAndGetString(httpContent);
            return JsonConvert.DeserializeObject<T>(contentString, settings);
        }

        private const string JsonMediaType = "application/json";

        private static Task<string> ValidateAndGetString(HttpContent httpContent)
        {
            var mediaType = httpContent.Headers.ContentType?.MediaType;
            if (!string.Equals(mediaType, JsonMediaType, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidMediaTypeException(JsonMediaType, mediaType);
            }

            return httpContent.ReadAsStringAsync();
        }
    }
}
