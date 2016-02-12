using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.Autofac.Test.TestObjects
{
    public class DomainEventWithHandlerRegistered : DomainEvent
    { }

    public class DomainEventWithHandlerRegisteredHandler : DomainEventHandler<DomainEventWithHandlerRegistered>
    {
        private readonly IDomainEventPublisher domainEventPublisher;

        public DomainEventWithHandlerRegisteredHandler(IDomainEventPublisherProvider domainEventPublisherProvider)
        {
            this.domainEventPublisher = domainEventPublisherProvider.GetPublisher();
        }

        public override Task HandleAsync(DomainEventWithHandlerRegistered domainEvent)
        {
            // Raised a second event that does not have any handlers registered to it
            return this.domainEventPublisher.RaiseAsync(new DomainEventNoHandlerRegistered());
        }
    }
}
