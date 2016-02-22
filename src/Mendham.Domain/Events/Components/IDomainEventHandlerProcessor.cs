using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events.Components
{
    public interface IDomainEventHandlerProcessor
    {
        /// <summary>
        /// Asynchronously processes the domain event for each of the handlers
        /// </summary>
        /// <typeparam name="TDomainEvent"></typeparam>
        /// <param name="domainEvent">Domain event</param>
        /// <param name="handlers">All registered handlers for the domain event type</param>
		/// <exception cref="DomainEventHandlingException">One or more errors occured by handler</exception>
        Task HandleAllAsync<TDomainEvent>(TDomainEvent domainEvent, IEnumerable<IDomainEventHandler<TDomainEvent>> handlers)
            where TDomainEvent : IDomainEvent;
    }
}
