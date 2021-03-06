﻿using Mendham.Domain;
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
    /// <typeparam name="TDomainFacade">Domain facade used by the entity under test</typeparam>
    /// <typeparam name="TBuilder">Builder that creates the entity to be tested</typeparam>
    public class EntityTest<TEntity, TDomainFacade, TBuilder> : Test<EntityFixture<TEntity, TDomainFacade, TBuilder>>
        where TEntity : IEntity
        where TDomainFacade : class, IDomainFacade
        where TBuilder : IEntityBuilder<TEntity, TDomainFacade, TBuilder>, new()
    {
        public EntityTest(EntityFixture<TEntity, TDomainFacade, TBuilder> fixture)
            : base(fixture)
        { }
    }
}
