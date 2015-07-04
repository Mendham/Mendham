using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBeall.Mendham.Domain
{
	public abstract class DomainFacade : IDomainFacade
	{
		private IDomainEventPublisher domainEventPublisher;

		public DomainFacade(IDomainEventPublisher domainEventPublisher)
		{
			this.domainEventPublisher = domainEventPublisher;
		}

		public Task RaiseEventAsync<TDomainEvent>(TDomainEvent domainEvent)
			where TDomainEvent : class, IDomainEvent
		{
			return domainEventPublisher.RaiseAsync(domainEvent);
		}
	}

	public interface IDomainFacade
	{
		Task RaiseEventAsync<TDomainEvent>(TDomainEvent domainEvent)
			where TDomainEvent : class, IDomainEvent;
	}
}