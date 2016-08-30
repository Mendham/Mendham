using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq
{
    public static class MockingFixtureExtensions
    {
        /// <summary>
        /// For a given fixture, replaces all public read/write properties with new mock values unless they are
        /// marked with a <see cref="IgnoreFixtureComponentAttribute"/>
        /// </summary>
        /// <param name="fixture"></param>
        public static void ResetMockProperties(this IFixture fixture)
        {
            foreach (var prop in GetProperties(fixture))
            {
                var mockType = typeof(Mock<>)
                    .MakeGenericType(prop.PropertyType);

                Mock mockObj = Activator.CreateInstance(mockType) as Mock;

                prop.SetValue(fixture, mockObj.Object);
            }
        }

        private static IEnumerable<PropertyInfo> GetProperties(IFixture fixture)
        {
            return fixture.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(IsComponentToMock)
                .ToList();
        }

        private static bool IsComponentToMock(PropertyInfo property)
        {
            return property.CanRead
                && property.CanWrite
                && property.GetGetMethod(false) != null
                && property.GetSetMethod(false) != null
                && !property.IsDefined(typeof(IgnoreFixtureComponentAttribute), false);
        }
    }
}
