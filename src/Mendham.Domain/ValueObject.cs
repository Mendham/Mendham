using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham.Equality;
using System.Reflection;

namespace Mendham.Domain
{
	public abstract class ValueObject : IHasEqualityComponents
	{
        private IEnumerable<PropertyInfo> _propertyInfo;

        public override bool Equals(object obj)
		{
			return this.EqualsFromComponents(obj);
		}

		public override int GetHashCode()
		{
			return this.GetHashCodeFromComponents();
		}

		public static bool operator ==(ValueObject a, ValueObject b)
		{
			return object.Equals(a, b);
		}

		public static bool operator !=(ValueObject a, ValueObject b)
		{
			return !(a == b);
		}

        private IEnumerable<PropertyInfo> GetPropertyInfo()
        {
            return this.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(a => a.CanRead && a.GetGetMethod(false) != null)
                .OrderBy(a => a.Name);
        }


		IEnumerable<object> IHasEqualityComponents.EqualityComponents
		{
			get
			{
                if (_propertyInfo == null)
                {
                    _propertyInfo = GetPropertyInfo();
                }

                return _propertyInfo.Select(a => a.GetValue(this));
			}
		}
	}
}