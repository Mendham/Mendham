using Mendham.Domain;

namespace Mendham.Testing
{
    public interface IEntityFixture<TEntity, TBuilder> : IFixture
        where TEntity : IEntity
        where TBuilder : IBuilder<TEntity>, new()
    {
        /// <summary>
        /// Gets the builder for the entity being tested
        /// </summary>
        /// <returns></returns>
        TBuilder GetSutBuilder();
    }

    public interface IEntityFixture<TEntity, TDomainFacade, TBuilder> : IEntityFixture<TEntity, TBuilder>
        where TEntity : IEntity
        where TDomainFacade : class, IDomainFacade
        where TBuilder : IEntityBuilder<TEntity, TDomainFacade, TBuilder>, new()
    {
        DomainEventPublisherFixture DomainEventPublisherFixture { get; set; }
        TDomainFacade DomainFacade { get; set; }

        /// <summary>
        /// Creates a default domain facade for the entity being tested
        /// </summary>
        /// <returns></returns>
        TDomainFacade BuildFacade();
    }
}