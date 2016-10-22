using Mendham.Domain;
using Mendham.Events;
using Moq;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq
{
    public class DomainFacadeMock
    {
        /// <summary>
        /// Generates a mockable implementation of the domain facade
        /// </summary>
        /// <typeparam name="TDomainFacade">Type of the domain facade</typeparam>
        /// <returns>Mockable implementation of the domain facade</returns>
        public static TDomainFacade Of<TDomainFacade>()
            where TDomainFacade : class, IDomainFacade
        {
            return Of<TDomainFacade>(new EmptyEventPublisher());
        }

        /// <summary>
        /// Generates a mockable implementation of the domain facade that utlizes a domain event publisher
        /// </summary>
        /// <typeparam name="TDomainFacade">Type of the domain facade</typeparam>
        /// <param name="eventPublisher">IEventPublisher to apply to facade</param>
        /// <returns>Mockable implementation of the domain facade</returns>
        public static TDomainFacade Of<TDomainFacade>(IEventPublisher eventPublisher)
            where TDomainFacade : class, IDomainFacade
        {
            return new DomainFacadeMockBase(eventPublisher)
                .As<TDomainFacade>()
                .Object;
        }

        /// <summary>
        /// Generates a mockable implementation of the domain facade that utlizes a event publisher defined in a fixture
        /// </summary>
        /// <typeparam name="TDomainFacade">Type of the domain facade</typeparam>
        /// <param name="eventPublisherFixture">A <see cref="EventPublisherFixture"/>that is used for tracking events raised within the fixture</param>
        /// <returns>Mockable implementation of the domain facade</returns>
        public static TDomainFacade Of<TDomainFacade>(EventPublisherFixture eventPublisherFixture)
            where TDomainFacade : class, IDomainFacade
        {
            var publisher = eventPublisherFixture.GetEventPublisher();
            return Of<TDomainFacade>(publisher);
        }

        private class EmptyEventPublisher : IEventPublisher
        {
            Task IEventPublisher.RaiseAsync<TEvent>(TEvent raisedEvent)
            {
                return Task.FromResult(0);
            }
        }

        private class DomainFacadeMockBase : Mock<DomainFacade>
        {
            public DomainFacadeMockBase(IEventPublisher domainEventPublisher)
                :base(domainEventPublisher)
            {
                CallBase = true;
            }
        }
    }
}