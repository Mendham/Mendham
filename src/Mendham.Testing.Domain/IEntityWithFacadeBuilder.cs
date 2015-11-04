using Mendham.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    public interface IEntityWithFacadeBuilder<TEntity, TFacade, TBuilder> : IBuilder<TEntity>
        where TEntity : class, IEntity
        where TFacade : class, IDomainFacade
        where TBuilder : IBuilder<TEntity>
    {
        TBuilder WithFacade(TFacade facade);
    }
}
