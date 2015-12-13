using Mendham.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain
{
    public static class ValueObjectEqualityExtensions
    {
        public static bool Equals<T>(this IValueObject<T> x, T y)
            where T : IValueObject<T>
        {
            if (ReferenceEquals(x, y))
                return true;

            return x.IsEqualToValueObject(y);
        }
    }
}
