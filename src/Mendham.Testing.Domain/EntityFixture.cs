using Mendham.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
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
}