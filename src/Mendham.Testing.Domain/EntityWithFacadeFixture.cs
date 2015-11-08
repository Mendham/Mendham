using Mendham.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    /// <summary>
    /// Fixture used for testing an entity with a domain facade.
    /// </summary>
    /// <typeparam name="TEntity">Entity being tested</typeparam>
    /// <typeparam name="TFacade">Domain facade used by the entity under test</typeparam>
    /// <typeparam name="TBuilder">Builder that creates the entity to be tested</typeparam>
    public class EntityWithFacadeFixture<TEntity, TFacade, TBuilder> : EntityFixture<TEntity, TBuilder>
        where TEntity : class, IEntity
        where TFacade : class, IDomainFacade
        where TBuilder : IEntityWithFacadeBuilder<TEntity, TFacade, TBuilder>, new()
    {
    }
}
