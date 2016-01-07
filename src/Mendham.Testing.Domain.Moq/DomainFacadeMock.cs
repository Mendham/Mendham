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
            var emptyProvider = Mock.Of<IDomainEventPublisherProvider>(
                ctx => ctx.GetPublisher() == new EmptyDomainEventPublisher());

            return Of<TDomainFacade>(emptyProvider);
        }

        /// <summary>
        /// Generates a mockable implementation of the domain facade that utlizes a domain event publisher
        /// </summary>
        /// <typeparam name="TDomainFacade">Type of the domain facade</typeparam>
        /// <param name="domainEventPublisherProvider">A IDomainEventPublisherProvider that is implemented to include an IDomainEventPublisher</param>
        /// <returns>Mockable implementation of the domain facade</returns>
        public static TDomainFacade Of<TDomainFacade>(IDomainEventPublisherProvider domainEventPublisherProvider)
            where TDomainFacade : class, IDomainFacade
        {
            return new DomainFacadeMockBase(domainEventPublisherProvider)
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
            var provider = domainEventPublisherFixture.GetDomainEventPublisherProvider();
            return Of<TDomainFacade>(provider);
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
            public DomainFacadeMockBase(IDomainEventPublisherProvider domainEventPublisherProvider)
                :base(domainEventPublisherProvider)
            {
                this.CallBase = true;
            }
        }
    }
}