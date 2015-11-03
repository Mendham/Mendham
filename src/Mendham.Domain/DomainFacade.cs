using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham.Domain.Events;

namespace Mendham.Domain
{
	public abstract class DomainFacade : IDomainFacade
	{
		private readonly IDomainEventPublisher domainEventPublisher;

		public DomainFacade(IDomainEventPublisherProvider domainEventPublisherProvider)
		{
			this.domainEventPublisher = domainEventPublisherProvider.GetPublisher();
		}

		public Task RaiseEventAsync<TDomainEvent>(TDomainEvent domainEvent)
			where TDomainEvent : class, IDomainEvent
		{
			return domainEventPublisher.RaiseAsync(domainEvent);
		}
	}
}