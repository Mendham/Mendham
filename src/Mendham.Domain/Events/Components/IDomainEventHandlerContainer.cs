using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events.Components
{
    public interface IDomainEventHandlerContainer
    {
        IEnumerable<IDomainEventHandler<TDomainEvent>> GetHandlers<TDomainEvent>()
            where TDomainEvent : IDomainEvent;
    }
}
