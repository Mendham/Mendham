using Mendham.Domain;
using Mendham.Domain.Events;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    public class DomainFacadeMock
    {
        public static TFacade Of<TFacade>()
            where TFacade : class, IDomainFacade
        {
            var emptyProvider = Mock.Of<IDomainEventPublisherProvider>(
                ctx => ctx.GetPublisher() == new EmptyDomainEventPublisher());

            return Of<TFacade>(emptyProvider);
        }

        public static TFacade Of<TFacade>(IDomainEventPublisherProvider domainEventPublisherProvider)
            where TFacade : class, IDomainFacade
        {
            return new DomainFacadeMockBase(domainEventPublisherProvider)
                .As<TFacade>()
                .Object;
        }

        public static TFacade Of<TFacade>(DomainEventPublisherFixture domainEventPublisherFixture)
            where TFacade : class, IDomainFacade
        {
            var provider = domainEventPublisherFixture.GetDomainEventPublisherProvider();
            return Of<TFacade>(provider);
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