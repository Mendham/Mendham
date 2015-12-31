using Mendham.Domain;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq
{
    /// <summary>
    /// Use this as a base class to test an entity with a domain facade.
    /// </summary>
    /// <typeparam name="TEntity">Entity being tested</typeparam>
    /// <typeparam name="TFacade">Domain facade used by the entity under test</typeparam>
    /// <typeparam name="TBuilder">Builder that creates the entity to be tested</typeparam>
    public class EntityUnitTest<TEntity, TFacade, TBuilder> : UnitTest<EntityFixture<TEntity, TFacade, TBuilder>>
        where TEntity : class, IEntity
        where TFacade : class, IDomainFacade
        where TBuilder : IEntityBuilder<TEntity, TFacade, TBuilder>, new()
    {
        public EntityUnitTest(EntityFixture<TEntity, TFacade, TBuilder> fixture)
            : base(fixture)
        { }
    }
}
