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
		private readonly IDomainEventLoggerContainer loggerContainer;

		public DomainEventPublisher(IDomainEventHandlerContainer handlerContainer,
            IDomainEventLoggerContainer loggerContainer)
		{
			this.handlerContainer = handlerContainer;
			this.loggerContainer = loggerContainer;
		}

		public Task RaiseAsync<TDomainEvent>(TDomainEvent domainEvent)
			where TDomainEvent : class, IDomainEvent
		{
            // Log Event
            loggerContainer.WriteToAllLoggers(domainEvent);

			// Handle Event
			return handlerContainer.HandleAllAsync(domainEvent);
		}
	}
}
