using System.Collections.Generic;

namespace Mendham.Domain.Events.Components
{
    public interface IDomainEventPublisherComponents
    {
        IDomainEventHandlerContainer DomainEventHandlerContainer { get; }
        IDomainEventHandlerProcessor DomainEventHandlerProcessor { get; }
        IEnumerable<IDomainEventLogger> DomainEventLoggers { get; }
    }
}