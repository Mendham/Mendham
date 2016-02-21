using Mendham.Domain.Events;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.TestObjects
{
    public class DomainEventWithHandlerRegistered : DomainEvent
    { }

    public class DomainEventWithHandlerRegisteredHandler : IDomainEventHandler<DomainEventWithHandlerRegistered>
    {
        private readonly IDomainEventPublisher domainEventPublisher;

        public DomainEventWithHandlerRegisteredHandler(IDomainEventPublisher domainEventPublisher)
        {
            this.domainEventPublisher = domainEventPublisher;
        }

        public Task HandleAsync(DomainEventWithHandlerRegistered domainEvent)
        {
            // Raised a second event that does not have any handlers registered to it
            return this.domainEventPublisher.RaiseAsync(new DomainEventNoHandlerRegistered());
        }
    }
}
