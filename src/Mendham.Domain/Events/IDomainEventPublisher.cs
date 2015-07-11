using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events
{
	/// <summary>
	/// Use this interface to raise domain events
	/// </summary>
	public interface IDomainEventPublisher
	{
		/// <summary>
		/// Raises a domain event to be logged and handled
		/// </summary>
		/// <typeparam name="TDomainEvent">Type of Domain Event</typeparam>
		/// <param name="domainEvent">Domain Event</param>
		/// <returns>Empty task after successfully</returns>
		/// <exception cref="DomainEventHandlingException">One or more errors occured by handler</exception>
		Task RaiseAsync<TDomainEvent>(TDomainEvent domainEvent)
			where TDomainEvent : class, IDomainEvent;
	}
}
