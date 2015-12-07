using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham.Equality;
using System.Reflection;

namespace Mendham.Domain
{
    /// <summary>
    /// Base class for value object that does not implement IEquatable. To have IEquatable already defined, use ValueObject<T>
    /// </summary>
    public abstract class ValueObject : IHasEqualityComponents
    {
        private IEnumerable<PropertyInfo> _propertyInfo;

        public sealed override bool Equals(object obj)
		{
            return this.HaveEqualComponents(obj) && this.IsObjectSameType(obj);
        }

        public sealed override int GetHashCode()
		{
			return this.GetHashCodeForObjectWithComponents();
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

    /// <summary>
    /// Base class for value objects that also implements IEquatable for the type
    /// </summary>
    /// <typeparam name="T">The derived type of ValueObject<T></typeparam>
    public abstract class ValueObject<T> : ValueObject, IEquatable<T>
        where T : ValueObject<T>, IHasEqualityComponents
    {
        public bool Equals(T other)
        {
            return ((T)this).HaveEqualComponents<T>(other) && this.IsObjectSameType(other);
        }

        public static explicit operator T(ValueObject<T> valueObject)
        {
            return valueObject as T;
        }
    }
}