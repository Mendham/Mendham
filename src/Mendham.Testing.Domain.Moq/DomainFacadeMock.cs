using Mendham.Domain;
using Mendham.Domain.Events;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return Of<TDomainFacade>(new EmptyDomainEventPublisher());
        }

        /// <summary>
        /// Generates a mockable implementation of the domain facade that utlizes a domain event publisher
        /// </summary>
        /// <typeparam name="TDomainFacade">Type of the domain facade</typeparam>
        /// <param name="domainEventPublisher">IDomainEventPublisher to apply to facade</param>
        /// <returns>Mockable implementation of the domain facade</returns>
        public static TDomainFacade Of<TDomainFacade>(IDomainEventPublisher domainEventPublisher)
            where TDomainFacade : class, IDomainFacade
        {
            return new DomainFacadeMockBase(domainEventPublisher)
                .As<TDomainFacade>()
                .Object;
        }

        /// <summary>
        /// Generates a mockable implementation of the domain facade that utlizes a domain event publisher defined in a fixture
        /// </summary>
        /// <typeparam name="TDomainFacade">Type of the domain facade</typeparam>
        /// <param name="domainEventPublisherFixture">A DomainEventPublisherFixture that is used for tracking domain events with raised within the fixture</param>
        /// <returns>Mockable implementation of the domain facade</returns>
        public static TDomainFacade Of<TDomainFacade>(DomainEventPublisherFixture domainEventPublisherFixture)
            where TDomainFacade : class, IDomainFacade
        {
            var publisher = domainEventPublisherFixture.GetDomainEventPublisher();
            return Of<TDomainFacade>(publisher);
        }

        private class EmptyDomainEventPublisher : IDomainEventPublisher
        {
            Task IDomainEventPublisher.RaiseAsync<TDomainEvent>(TDomainEvent domainEvent)
            {
                return Task.FromResult(0);
            }
        }

        private class DomainFacadeMockBase : Mock<DomainFacade>
        {
            public DomainFacadeMockBase(IDomainEventPublisher domainEventPublisher)
                :base(domainEventPublisher)
            {
                this.CallBase = true;
            }
        }
    }
}