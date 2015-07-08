using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events
{
    public interface IDomainEventHandlerContainer
    {
		Task HandleAllAsync<TDomainEvent>(TDomainEvent domainEvent)
			where TDomainEvent : IDomainEvent;
    }
}
