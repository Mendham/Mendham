using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events
{
	/// <summary>
	/// Base Domain Event Handler used to register domain event handlers
	/// </summary>
	public interface IDomainEventHandler
	{ }

	/// <summary>
	/// The interface for a domain event handler that handles the type of domain event as 
	/// well as any type derived from the domain event
	/// </summary>
	/// <typeparam name="TDomainEvent">Type of domain event to be handled</typeparam>
	public interface IDomainEventHandler<TDomainEvent> : IDomainEventHandler
		where TDomainEvent : IDomainEvent
	{
		/// <summary>
		/// Executes the Domain Event Handler
		/// </summary>
		/// <param name="domainEvent">The domain event to be handled.</param>
		/// <returns>A task that represents the completion of the domain event handler</returns>
		Task HandleAsync(TDomainEvent domainEvent);
	}

	public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
		where TDomainEvent : IDomainEvent
	{
		/// <summary>
		/// Executes the Domain Event Handler
		/// </summary>
		/// <param name="domainEvent">The domain event to be handled.</param>
		/// <returns>A task that represents the completion of the domain event handler</returns>
		public abstract Task HandleAsync(TDomainEvent domainEvent);
	}
}