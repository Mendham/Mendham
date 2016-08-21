using System;
using System.Threading.Tasks;

namespace Mendham.Events.Components
{
    public class EventPublisher : IEventPublisher
	{
        private readonly Func<IEventPublisherComponents> _eventPublisherComponentsFactory;

        public EventPublisher(Func<IEventPublisherComponents> eventPublisherContainerFactory)
		{
			_eventPublisherComponentsFactory = eventPublisherContainerFactory;
		}

		public Task RaiseAsync<TEvent>(TEvent eventRaised)
			where TEvent : class, IEvent
		{
            var eventPublisherComponents = _eventPublisherComponentsFactory();

            // Log Event
            eventPublisherComponents.EventLoggerProcessor.LogEvent(eventRaised);

            // Get Handlers
            var handlers = eventPublisherComponents.EventHandlerContainer.GetHandlers<TEvent>();

            // Get task to process all handlers
            return eventPublisherComponents.EventHandlerProcessor.HandleAllAsync(eventRaised, handlers);
        }
    }
}
