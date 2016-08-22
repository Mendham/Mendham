using System.Threading.Tasks;

namespace Mendham.Events.DependencyInjection.TestObjects
{
    public class EventWithHandlerRegistered : Event
    { }

    public class DomainEventWithHandlerRegisteredHandler : IEventHandler<EventWithHandlerRegistered>
    {
        private readonly IEventPublisher _eventPublisher;

        public DomainEventWithHandlerRegisteredHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public Task HandleAsync(EventWithHandlerRegistered domainEvent)
        {
            // Raised a second event that does not have any handlers registered to it
            return _eventPublisher.RaiseAsync(new EventNoHandlerRegistered());
        }
    }
}
