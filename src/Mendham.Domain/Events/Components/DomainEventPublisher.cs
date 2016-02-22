using System;
using System.Threading.Tasks;

namespace Mendham.Domain.Events.Components
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
            domainEventPublisherComponents.DomainEventLoggerProcessor.LogDomainEvent(domainEvent);

            // Get Handlers
            var handlers = domainEventPublisherComponents.DomainEventHandlerContainer.GetHandlers<TDomainEvent>();

            // Get task to process all handlers
            return domainEventPublisherComponents.DomainEventHandlerProcessor.HandleAllAsync(domainEvent, handlers);
        }
    }
}
