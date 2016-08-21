namespace Mendham.Events.Components
{
    public class EventPublisherComponents : IEventPublisherComponents
    {
        private readonly IEventHandlerContainer _eventHandlerContainer;
        private readonly IEventHandlerProcessor _eventHandlerProcessor;
        private readonly IEventLoggerProcessor _eventLoggerProcessor;

        public EventPublisherComponents(IEventHandlerContainer eventHandlerContainer,
            IEventHandlerProcessor eventHandlerProcessor,
            IEventLoggerProcessor eventLoggerProcessor)
        {
            _eventHandlerContainer = eventHandlerContainer;
            _eventHandlerProcessor = eventHandlerProcessor;
            _eventLoggerProcessor = eventLoggerProcessor;
        }

        public IEventHandlerContainer EventHandlerContainer
        {
            get
            {
                return _eventHandlerContainer;
            }
        }

        public IEventHandlerProcessor EventHandlerProcessor
        {
            get
            {
                return _eventHandlerProcessor;
            }
        }

        public IEventLoggerProcessor EventLoggerProcessor
        {
            get
            {
                return _eventLoggerProcessor;
            }
        }
    }
}
