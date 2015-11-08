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
    public class EntityFixture<TEntity, TBuilder> : IEntityFixture<TEntity, TBuilder> 
        where TEntity : class, IEntity
        where TBuilder : IBuilder<TEntity>, new()
    {
        public virtual TBuilder GetSutBuilder()
        {
            return new TBuilder();
        }

        public virtual void ResetFixture()
        { }
    }
}