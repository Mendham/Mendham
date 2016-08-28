using System;
using System.Threading.Tasks;

namespace Mendham.Events.Components
{
    public class EventPublisher : IEventPublisher
	{
        private readonly IEventPublisherComponents _eventPublisherComponents;

        public EventPublisher(IEventPublisherComponents eventPublisherContainer)
		{
			_eventPublisherComponents = eventPublisherContainer;
		}

		public Task RaiseAsync<TEvent>(TEvent eventRaised) where TEvent : class, IEvent
		{
            // Log Event
            _eventPublisherComponents.EventLoggerProcessor.LogEvent(eventRaised);

            // Get Handlers
            var handlers = _eventPublisherComponents.EventHandlerContainer.GetHandlers<TEvent>();

            // Get task to process all handlers
            return _eventPublisherComponents.EventHandlerProcessor.HandleAllAsync(eventRaised, handlers);
        }
    }
}
