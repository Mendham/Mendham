﻿using Mendham.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    /// <summary>
    /// Contact for an IBuilder that builds an entity
    /// </summary>
    /// <typeparam name="TEntity">Entity being built</typeparam>
    public interface IEntityBuilder<TEntity> : IBuilder<TEntity>
        where TEntity : class, IEntity
    {
    }

    /// <summary>
    /// Contact for an IBuilder that builds an entity with a domain facade
    /// </summary>
    /// <typeparam name="TEntity">Entity being built</typeparam>
    /// <typeparam name="TFacade">Domain facade of entity being built</typeparam>
    /// <typeparam name="TBuilder">Builder that creates the entity</typeparam>
    public interface IEntityBuilder<TEntity, TFacade, TBuilder> : IEntityBuilder<TEntity>
        where TEntity : class, IEntity
        where TFacade : class, IDomainFacade
        where TBuilder : IBuilder<TEntity>
    {
        TBuilder WithFacade(TFacade facade);
    }
}