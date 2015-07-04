using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBeall.Mendham.Domain
{
	public interface IDomainEventHandler
	{ }

	public interface IDomainEventHandler<TDomainEvent> : IDomainEventHandler
		where TDomainEvent : IDomainEvent
	{
		Task HandleAsync(TDomainEvent domainEvent);
	}

	public abstract class BaseDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
		where TDomainEvent : IDomainEvent
	{
		public abstract Task HandleAsync(TDomainEvent domainEvent);

		public Type GetDomainEventType()
		{
			return typeof(TDomainEvent);
		}
	}
}
