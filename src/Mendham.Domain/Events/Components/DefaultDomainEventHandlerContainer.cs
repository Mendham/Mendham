using System;
using System.Collections.Generic;

namespace Mendham.Domain.Events.Components
{
    public class DefaultDomainEventHandlerContainer : IDomainEventHandlerContainer
    {
        private readonly Func<IEnumerable<IDomainEventHandler>> domainEventHandlersFactory;

        public DefaultDomainEventHandlerContainer(Func<IEnumerable<IDomainEventHandler>> domainEventHandlersFactory)
        {
            this.domainEventHandlersFactory = domainEventHandlersFactory;
        }

        public IEnumerable<IDomainEventHandler<TDomainEvent>> GetHandlers<TDomainEvent>() where TDomainEvent : IDomainEvent
        {
            return domainEventHandlersFactory()
                .SelectHandlersForDomainEvent<TDomainEvent>();
        }
    }
}
