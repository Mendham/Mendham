using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham;
using System.Reflection;

namespace Mendham.Domain.Events
{
	public class DomainEventPublisher : IDomainEventPublisher
	{
		private readonly IDomainEventHandlerContainer handlerContainer;
		private readonly IEnumerable<IDomainEventLogger> domainEventLoggers;

		public DomainEventPublisher(IDomainEventHandlerContainer handlerContainer,
			IEnumerable<IDomainEventLogger> domainEventLoggers)
		{
			this.handlerContainer = handlerContainer;
			this.domainEventLoggers = domainEventLoggers;
		}

		public Task RaiseAsync<TDomainEvent>(TDomainEvent domainEvent)
			where TDomainEvent : class, IDomainEvent
		{
			// Log Event
			foreach (var logger in domainEventLoggers)
				logger.LogDomainEvent(domainEvent);

			// Handle Event
			return handlerContainer.HandleAllAsync(domainEvent);
		}
	}
}
