using Mendham.Concurrency;
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
}
