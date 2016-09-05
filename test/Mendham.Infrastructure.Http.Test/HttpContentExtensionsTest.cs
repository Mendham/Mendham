using FluentAssertions;
using Mendham.Infrastructure.Http.Test.TestObjects;
using Mendham.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Infrastructure.Http.Test
{
    public class HttpContentExtensionsTest
    {
        [Theory]
        [MendhamData]
        public async Task ReadAsJson_ValidObject_ObjectEquals(SimpleObject expectedObject)
        {
            var serializedObject = string.Format(SimpleObject.FormatString, 
                expectedObject.Value1, expectedObject.Value2);
            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var result = await content.ReadAsJsonAsync<SimpleObject>();

            result.Should()
                .Be(expectedObject);
        }

        [Theory]
        [MendhamData]
        public async Task ReadAsJson_InvalidMediaType_ThrowsInvalidMediaTypeException(SimpleObject expectedObject)
        {
            var invalidMediaType = "text/plain";

            var serializedObject = string.Format(SimpleObject.FormatString,
                expectedObject.Value1, expectedObject.Value2);
            var content = new StringContent(serializedObject, Encoding.UTF8, invalidMediaType);

            var ex = await Assert.ThrowsAsync<InvalidMediaTypeException>(() => content.ReadAsJsonAsync<SimpleObject>());

            ex.ActualMediaType.Should()
                .Be(invalidMediaType);
            ex.ExpectedMediaType.Should()
                .Be("application/json");
        }

        [Theory]
        [MendhamData]
        public async Task ReadAsJsonWithConverters_ValidObject_ObjectPropertiesAsExpected(string value1, int value2)
        {
            var expectedObject = new ObjectWithReadOnlyProperties(value1, value2);

            var serializedObject = string.Format(ObjectWithReadOnlyProperties.FormatStringWithAdditionalContent,
                expectedObject.Value1, expectedObject.Value2, expectedObject.ValueNotToSerialize);
            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var result = await content.ReadAsJsonAsync<ObjectWithReadOnlyProperties>(new ObjectWithReadOnlyPropertiesConverter());

            result.Value1.Should()
                .Be(value1, "the value was properly deserialized by the converter");
            result.Value2.Should()
                .Be(value2, "the value was properly deserialized by the converter");
            result.ValueNotToSerialize.Should()
                .NotBe(expectedObject.ValueNotToSerialize, "the value was not handled by converter and should be regenerated at random");
        }

        [Theory]
        [MendhamData]
        public async Task ReadAsJsonWithConverters_InvalidMediaType_ThrowsInvalidMediaTypeException(SimpleObject expectedObject)
        {
            var invalidMediaType = "text/plain";

            var serializedObject = string.Format(SimpleObject.FormatString,
                expectedObject.Value1, expectedObject.Value2);
            var content = new StringContent(serializedObject, Encoding.UTF8, invalidMediaType);

            var ex = await Assert.ThrowsAsync<InvalidMediaTypeException>(() => 
                content.ReadAsJsonAsync<SimpleObject>(new ObjectWithReadOnlyPropertiesConverter()));

            ex.ActualMediaType.Should()
                .Be(invalidMediaType);
            ex.ExpectedMediaType.Should()
                .Be("application/json");
        }

        [Theory]
        [MendhamData]
        public async Task ReadAsJsonWithSerializerSettings_ValidObject_ObjectPropertiesAsExpected(string value1, int value2)
        {
            var expectedObject = new ObjectWithReadOnlyProperties(value1, value2);

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new ObjectWithReadOnlyPropertiesConverter());

            var serializedObject = string.Format(ObjectWithReadOnlyProperties.FormatStringWithAdditionalContent,
                expectedObject.Value1, expectedObject.Value2, expectedObject.ValueNotToSerialize);
            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var result = await content.ReadAsJsonAsync<ObjectWithReadOnlyProperties>(jsonSerializerSettings);

            result.Value1.Should()
                .Be(value1, "the value was properly deserialized by the JsonSerializerSettings");
            result.Value2.Should()
                .Be(value2, "the value was properly deserialized by the JsonSerializerSettings");
            result.ValueNotToSerialize.Should()
                .NotBe(expectedObject.ValueNotToSerialize, "the value was not handled by JsonSerializerSettings and should be regenerated at random");
        }

        [Theory]
        [MendhamData]
        public async Task ReadAsJsonWithSerializerSettings_InvalidMediaType_ThrowsInvalidMediaTypeException(SimpleObject expectedObject)
        {
            var invalidMediaType = "text/plain";

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new ObjectWithReadOnlyPropertiesConverter());

            var serializedObject = string.Format(SimpleObject.FormatString,
                expectedObject.Value1, expectedObject.Value2);
            var content = new StringContent(serializedObject, Encoding.UTF8, invalidMediaType);

            var ex = await Assert.ThrowsAsync<InvalidMediaTypeException>(() =>
                content.ReadAsJsonAsync<SimpleObject>(jsonSerializerSettings));

            ex.ActualMediaType.Should()
                .Be(invalidMediaType);
            ex.ExpectedMediaType.Should()
                .Be("application/json");
        }
    }
}
