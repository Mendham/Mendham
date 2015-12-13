using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain
{
    public interface IValueObject<T> : IHasEqualityComponents, IEquatable<T>
        where T : IValueObject<T>
    {
    }
}
