using Mendham.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    /// <summary>
    /// Fixture used for testing an Entity
    /// </summary>
    /// <typeparam name="TEntity">Entity being tested</typeparam>
    /// <typeparam name="TBuilder">Builder that creates the entity to be tested</typeparam>
    public class EntityFixture<TEntity, TBuilder> : IFixture
        where TEntity : class, IEntity
        where TBuilder : IBuilder<TEntity>, new()
    {
        public virtual TBuilder GetEntityBuilder()
        {
            return new TBuilder();
        }

        public virtual void ResetFixture()
        { }
    }

    /// <summary>
    /// Fixture used for testing an entity with a domain facade.
    /// </summary>
    /// <typeparam name="TEntity">Entity being tested</typeparam>
    /// <typeparam name="TFacade">Domain facade used by the entity under test</typeparam>
    /// <typeparam name="TBuilder">Builder that creates the entity to be tested</typeparam>
    public class EntityFixture<TEntity, TFacade, TBuilder> : EntityFixture<TEntity, TBuilder>
        where TEntity : class, IEntity
        where TFacade : class, IDomainFacade
        where TBuilder : IEntityBuilder<TEntity, TFacade, TBuilder>, new()
    {
    }
}