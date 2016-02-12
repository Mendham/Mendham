using Mendham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Mendham.Domain.Events
{
	public class DomainEventPublisher : IDomainEventPublisher
	{
        private readonly Func<IDomainEventPublisherComponents> domainEventPublisherComponentsFactory;

        public DomainEventPublisher(Func<IDomainEventPublisherComponents> domainEventPublisherContainerFactory)
		{
			this.domainEventPublisherComponentsFactory = domainEventPublisherContainerFactory;
		}

		public Task RaiseAsync<TDomainEvent>(TDomainEvent domainEvent)
			where TDomainEvent : class, IDomainEvent
		{
            var domainEventPublisherComponents = domainEventPublisherComponentsFactory();

            // Log Event
            domainEventPublisherComponents.DomainEventLoggerContainer.WriteToAllLoggers(domainEvent);

			// Handle Event
			return domainEventPublisherComponents.DomainEventHandlerContainer.HandleAllAsync(domainEvent);
		}
    }
}
