using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain
{
	public interface IDomainEventPublisher
	{
		Task RaiseAsync<TDomainEvent>(TDomainEvent domainEvent)
			where TDomainEvent : class, IDomainEvent;
	}
}
