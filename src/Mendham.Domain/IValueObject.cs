using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain
{
    public interface IValueObject
    { }

    public interface IValueObject<TValueObject> : IValueObject, IEquatable<TValueObject>
        where TValueObject : IValueObject
    { }
}
