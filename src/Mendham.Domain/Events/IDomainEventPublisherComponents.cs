using System.Collections.Generic;

namespace Mendham.Domain.Events
{
    public interface IDomainEventPublisherComponents
    {
        IDomainEventHandlerContainer DomainEventHandlerContainer { get; }
        IEnumerable<IDomainEventLogger> DomainEventLoggers { get; }
    }
}