using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Extensions
{
    public static class ValueObjectExtensions
    {
        public static bool IsEqualToValueObject<T>(this IValueObject<T> valueObject, object obj)
            where T : IValueObject<T>
        {
            if (ReferenceEquals(valueObject, obj))
                return true;

            if (obj is T)
            {
                return valueObject.Equals<T>((T)obj);
            }
            else
            {
                return false;
            }
        }

        public static bool IsEqualToValueObject<T>(this IValueObject<T> valueObject, T obj)
            where T : IValueObject<T>
        {
            return valueObject.Equals<T>(obj);
        }

        public static int GetValueObjectHashCode<T>(this T valueObject)
            where T : IValueObject<T>
        {
            return valueObject.GetObjectWithEqualityComponentsHashCode();
        }
    }
}
