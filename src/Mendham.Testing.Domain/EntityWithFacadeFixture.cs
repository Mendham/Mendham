using Mendham.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    public class EntityWithFacadeFixture<TEntity, TFacade, TBuilder> : EntityFixture<TEntity, TBuilder>
        where TEntity : class, IEntity
        where TFacade : class, IDomainFacade
        where TBuilder : IEntityWithFacadeBuilder<TEntity, TFacade, TBuilder>, new()
    {
    }
}
