using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mendham.Domain
{
    /// <summary>
    /// Base class for value object that does not implement IEquatable. To have IEquatable already defined, use ValueObject<T>
    /// </summary>
    public abstract class ValueObject : IHasEqualityComponents
    {
        private IEnumerable<Func<object>> _propertyValues;

        public override bool Equals(object obj)
		{
            if (ReferenceEquals(this, obj))
                return true;

            return this.AreComponentsEqual(obj) && this.IsObjectSameType(obj);
        }

        public override int GetHashCode()
		{
			return this.GetObjectWithEqualityComponentsHashCode();
		}

		public static bool operator ==(ValueObject a, ValueObject b)
		{
			return Equals(a, b);
		}

		public static bool operator !=(ValueObject a, ValueObject b)
		{
			return !(a == b);
		}

        private IEnumerable<Func<object>> GetPropertyValues()
        {
            return this.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(a => a.CanRead && a.GetGetMethod(false) != null)
                .Select<PropertyInfo, Func<object>>(a => () => a.GetValue(this))
                .ToList();
        }

		IEnumerable<object> IHasEqualityComponents.EqualityComponents
		{
			get
			{
                if (_propertyValues == null)
                {
                    _propertyValues = GetPropertyValues();
                }

                return _propertyValues
                    .Select(a => a());
			}
		}
	}

    /// <summary>
    /// Base class for value objects that also implements IEquatable for the type
    /// </summary>
    /// <typeparam name="T">The derived type of ValueObject<T></typeparam>
    public abstract class ValueObject<T> : ValueObject, IValueObject<T>, IEquatable<T>
        where T : ValueObject<T>
    {
        public bool Equals(T other)
        {
            return this.Equals<T>(other);
        }

        public static explicit operator T(ValueObject<T> valueObject)
        {
            return valueObject as T;
        }
    }
}