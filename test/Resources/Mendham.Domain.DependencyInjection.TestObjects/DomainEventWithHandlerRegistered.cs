using Mendham.Domain.Events;
using Mendham.Events;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.TestObjects
{
    public class DomainEventWithHandlerRegistered : DomainEvent
    { }

    public class DomainEventWithHandlerRegisteredHandler : IEventHandler<DomainEventWithHandlerRegistered>
    {
        private readonly IEventPublisher _eventPublisher;

        public DomainEventWithHandlerRegisteredHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public Task HandleAsync(DomainEventWithHandlerRegistered domainEvent)
        {
            // Raised a second event that does not have any handlers registered to it
            return _eventPublisher.RaiseAsync(new DomainEventNoHandlerRegistered());
        }
    }
}
