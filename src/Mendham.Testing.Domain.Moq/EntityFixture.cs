using Mendham.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq
{
    /// <summary>
    /// Fixture used for testing an entity with a domain facade.
    /// </summary>
    /// <typeparam name="TEntity">Entity being tested</typeparam>
    /// <typeparam name="TFacade">Domain facade used by the entity under test</typeparam>
    /// <typeparam name="TBuilder">Builder that creates the entity to be tested</typeparam>
    public class EntityFixture<TEntity, TFacade, TBuilder> : EntityFixture<TEntity, TBuilder>, IEntityFixture<TEntity, TFacade, TBuilder>
        where TEntity : class, IEntity
        where TFacade : class, IDomainFacade
        where TBuilder : IEntityBuilder<TEntity, TFacade, TBuilder>, new()
    {
        public DomainEventPublisherFixture DomainEventPublisherFixture { get; set; }
        public TFacade Facade { get; set; }

        public EntityFixture()
        {
            this.DomainEventPublisherFixture = new DomainEventPublisherFixture();
            this.Facade = BuildFacade();
        }

        /// <summary>
        /// Gets the builder for the entity being tested that utilizes facade and domain event publisher 
        /// assocaited with this fixture
        /// </summary>
        /// <returns></returns>
        public override TBuilder GetSutBuilder()
        {
            return base.GetSutBuilder()
                .WithFacade(this.Facade);
        }

        /// <summary>
        /// Prepares fixture for new test to be run
        /// </summary>
        public override void ResetFixture()
        {
            base.ResetFixture();

            this.DomainEventPublisherFixture.ResetFixture();
            this.Facade = BuildFacade();
        }

        protected virtual TFacade BuildFacade()
        {
            return DomainFacadeMock.Of<TFacade>(this.DomainEventPublisherFixture);
        }

        TFacade IEntityFixture<TEntity, TFacade, TBuilder>.BuildFacade()
        {
            return BuildFacade();
        }
    }
}
