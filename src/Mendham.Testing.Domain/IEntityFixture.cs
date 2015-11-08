using Mendham.Domain;

namespace Mendham.Testing
{
    public interface IEntityFixture<TEntity, TBuilder> : IFixture
        where TEntity : class, IEntity
        where TBuilder : IBuilder<TEntity>, new()
    {
        /// <summary>
        /// Gets the builder for the entity being tested
        /// </summary>
        /// <returns></returns>
        TBuilder GetSutBuilder();
    }

    public interface IEntityFixture<TEntity, TFacade, TBuilder> : IEntityFixture<TEntity, TBuilder>
        where TEntity : class, IEntity
        where TFacade : class, IDomainFacade
        where TBuilder : IEntityBuilder<TEntity, TFacade, TBuilder>, new()
    {
        DomainEventPublisherFixture DomainEventPublisherFixture { get; set; }
        TFacade Facade { get; set; }

        /// <summary>
        /// Creates a default domain facade for the entity being tested
        /// </summary>
        /// <returns></returns>
        TFacade BuildFacade();
    }
}