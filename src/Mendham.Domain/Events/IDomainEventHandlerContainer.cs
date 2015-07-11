using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events
{
    public interface IDomainEventHandlerContainer
    {
		/// <summary>
		/// Handles all associated domain event handlers
		/// </summary>
		/// <typeparam name="TDomainEvent">Type of domain event</typeparam>
		/// <param name="domainEvent">Domain Event</param>
		/// <returns>A task that represents the completion of all domain event handlers</returns>
		/// <exception cref="DomainEventHandlingException">One or more errors occured by handler</exception>
		Task HandleAllAsync<TDomainEvent>(TDomainEvent domainEvent)
			where TDomainEvent : IDomainEvent;
    }
}
