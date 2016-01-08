using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq
{
    public abstract class MockingFixture<T> : Fixture<T>, IFixture<T>
    {
        private List<PropertyInfo> properties;

        public sealed override void ResetFixture()
        {
            foreach(var prop in GetProperties())
            {
                var mockType = typeof(Mock<>)
                    .MakeGenericType(prop.PropertyType);

                Mock mockObj = Activator.CreateInstance(mockType) as Mock;

                prop.SetValue(this, mockObj.Object);
            }
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            if (properties == null)
            {
                properties = this.GetType()
                    .GetTypeInfo()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(IsComponentToMock)
                    .ToList();
            }

            return properties;
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
