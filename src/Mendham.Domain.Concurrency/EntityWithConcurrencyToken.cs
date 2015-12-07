using Mendham.Concurrency;
using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Concurrency
{
    public abstract class EntityWithConcurrencyToken : Entity, IHasConcurrencyToken
    {
        IConcurrencyToken IHasConcurrencyToken.Token { get; set; }
    }

    public abstract class EntityWithConcurrencyToken<T> : Entity<T>, IHasConcurrencyToken
        where T : EntityWithConcurrencyToken<T>, IHasEqualityComponents
    {
        IConcurrencyToken IHasConcurrencyToken.Token { get; set; }
    }
}
