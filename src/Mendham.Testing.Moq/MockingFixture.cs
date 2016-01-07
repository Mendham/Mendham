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
                    .Where(prop => prop.IsDefined(typeof(FixtureComponentAttribute), false))
                    .ToList();
            }

            return properties;
        }

    }
}
