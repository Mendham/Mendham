using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain
{
    public interface IEntity : IHasEqualityComponents
    { }

    public interface IEntity<TEntity> : IEntity, IEquatable<TEntity>
        where TEntity : IEntity
    { }
}
