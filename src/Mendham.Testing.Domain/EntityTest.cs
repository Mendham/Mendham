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
    public class EntityTest<TEntity, TBuilder> : Test<EntityFixture<TEntity, TBuilder>>
        where TEntity : class, IEntity
        where TBuilder : IBuilder<TEntity>, new()
    {
        public EntityTest(EntityFixture<TEntity, TBuilder> fixture)
            : base(fixture)
        { }
    }
}
