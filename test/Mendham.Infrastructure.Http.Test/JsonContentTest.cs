using FluentAssertions;
using Mendham.Infrastructure.Http.Test.TestObjects;
using Mendham.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Infrastructure.Http.Test
{
    public class JsonContentTest
    {
        private const string JsonMediaType = "application/json";

        [Fact]
        public async Task JsonContentConstructor_ValidSerializedContent_ContainsValidContent()
        {
            var serializedContent = "{\"value1\":\"abc\",\"value2\":\"1\"}";

            var sut = new JsonContent(serializedContent);

            var result = await sut.ReadAsStringAsync();

            result.Should()
                .Be(serializedContent, "that was the value passed");
        }

        [Fact]
        public void JsonContentConstructor_ValidSerializedContent_JsonMediaType()
        {
            var serializedContent = "{\"value1\":\"abc\",\"value2\":\"1\"}";

            var sut = new JsonContent(serializedContent);

            sut.Headers.ContentType.MediaType.Should()
                .Be(JsonMediaType, "that is the value required for json");
        }

        [Theory]
        [MendhamData]
        public async Task FromObject_ValidObject_ObjectEquals(SimpleObject obj)
        {
            var expectedResult = string.Format(SimpleObject.FormatString,
                obj.Value1, obj.Value2);

            var sut = JsonContent.FromObject(obj);

            var result = await sut.ReadAsStringAsync();

            result.Should()
                .BeEquivalentTo(expectedResult);
        }

        [Theory]
        [MendhamData]
        public async Task FromObjectWithConverters_ValidObject_ObjectAsExpected(string value1, int value2)
        {
            var obj = new ObjectWithReadOnlyProperties(value1, value2);

            var expectedResult = string.Format(ObjectWithReadOnlyProperties.FormatString,
                obj.Value1, obj.Value2);

            var sut = JsonContent.FromObject(obj, new ObjectWithReadOnlyPropertiesConverter());

            var result = await sut.ReadAsStringAsync();

            result.Should()
                .BeEquivalentTo(expectedResult, "the object was properly serialized by the converter");
        }

        [Theory]
        [MendhamData]
        public async Task FromObjectWithSerializerSettings_ValidObject_ObjectAsExpected(string value1, int value2)
        {
            var obj = new ObjectWithReadOnlyProperties(value1, value2);

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new ObjectWithReadOnlyPropertiesConverter());

            var expectedResult = string.Format(ObjectWithReadOnlyProperties.FormatString,
                obj.Value1, obj.Value2);

            var sut = JsonContent.FromObject(obj, jsonSerializerSettings);

            var result = await sut.ReadAsStringAsync();

            result.Should()
                .BeEquivalentTo(expectedResult, "the object was properlty serialized using the serializer settings");
        }
    }
}
