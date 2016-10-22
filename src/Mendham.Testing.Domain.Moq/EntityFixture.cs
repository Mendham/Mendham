using Mendham.Domain;

namespace Mendham.Testing.Moq
{
    /// <summary>
    /// Fixture used for testing an entity with a domain facade.
    /// </summary>
    /// <typeparam name="TEntity">Entity being tested</typeparam>
    /// <typeparam name="TDomainFacade">Domain facade used by the entity under test</typeparam>
    /// <typeparam name="TBuilder">Builder that creates the entity to be tested</typeparam>
    public class EntityFixture<TEntity, TDomainFacade, TBuilder> : EntityFixture<TEntity, TBuilder>, IEntityFixture<TEntity, TDomainFacade, TBuilder>
        where TEntity : IEntity
        where TDomainFacade : class, IDomainFacade
        where TBuilder : IEntityBuilder<TEntity, TDomainFacade, TBuilder>, new()
    {
        public EventPublisherFixture DomainEventPublisherFixture { get; set; }
        public TDomainFacade DomainFacade { get; set; }

        public EntityFixture()
        {
            DomainEventPublisherFixture = new EventPublisherFixture();
            DomainFacade = BuildDomainFacade();
        }

        /// <summary>
        /// Gets the builder for the entity being tested that utilizes facade and domain event publisher 
        /// assocaited with this fixture
        /// </summary>
        /// <returns></returns>
        public override TBuilder GetSutBuilder()
        {
            return base.GetSutBuilder()
                .WithFacade(DomainFacade);
        }

        /// <summary>
        /// Prepares fixture for new test to be run
        /// </summary>
        public override void ResetFixture()
        {
            base.ResetFixture();

            DomainEventPublisherFixture.ResetFixture();
            DomainFacade = BuildDomainFacade();
        }

        protected virtual TDomainFacade BuildDomainFacade()
        {
            return DomainFacadeMock.Of<TDomainFacade>(DomainEventPublisherFixture);
        }

        TDomainFacade IEntityFixture<TEntity, TDomainFacade, TBuilder>.BuildFacade()
        {
            return BuildDomainFacade();
        }
    }
}
