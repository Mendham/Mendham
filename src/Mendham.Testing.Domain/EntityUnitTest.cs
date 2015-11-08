using Mendham.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing
{
    /// <summary>
    /// Use this as a base class to test an entity
    /// </summary>
    /// <typeparam name="TEntity">Entity being tested</typeparam>
    /// <typeparam name="TBuilder">Builder that creates the entity to be tested</typeparam>
    public class EntityUnitTest<TEntity, TBuilder> : BaseUnitTest<EntityFixture<TEntity, TBuilder>>
        where TEntity : class, IEntity
        where TBuilder : IBuilder<TEntity>, new()
    {
        public EntityUnitTest(EntityFixture<TEntity, TBuilder> fixture)
            : base(fixture)
        { }
    }

    /// <summary>
    /// Use this as a base class to test an entity with a domain facade.
    /// </summary>
    /// <typeparam name="TEntity">Entity being tested</typeparam>
    /// <typeparam name="TFacade">Domain facade used by the entity under test</typeparam>
    /// <typeparam name="TBuilder">Builder that creates the entity to be tested</typeparam>
    public class EntityUnitTest<TEntity, TFacade, TBuilder> : BaseUnitTest<EntityWithFacadeFixture<TEntity, TFacade, TBuilder>>
        where TEntity : class, IEntity
        where TFacade : class, IDomainFacade
        where TBuilder : IEntityWithFacadeBuilder<TEntity, TFacade, TBuilder>, new()
    {
        public EntityUnitTest(EntityWithFacadeFixture<TEntity, TFacade, TBuilder> fixture)
            : base(fixture)
        { }
    }
}
