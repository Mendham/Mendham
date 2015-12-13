using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain
{
    public interface IEntity
    {
        IEnumerable<object> IdentityComponents { get; }
    }

    public interface IEntity<TIdentity> : IEntity
    {
        TIdentity Id { get; }
    }
}
