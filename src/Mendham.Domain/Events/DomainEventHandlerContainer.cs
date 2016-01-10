using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Domain.Events
{
	public class DomainEventHandlerContainer : IDomainEventHandlerContainer
	{
		private IEnumerable<IDomainEventHandler> domainEventHandlers;

		public DomainEventHandlerContainer(IEnumerable<IDomainEventHandler> domainEventHandlers)
		{
			this.domainEventHandlers = domainEventHandlers;
		}

		/// <summary>
		/// Handles all associated domain event handlers
		/// </summary>
		/// <typeparam name="TDomainEvent">Type of domain event</typeparam>
		/// <param name="domainEvent">Domain Event</param>
		/// <returns>A task that represents the completion of all domain event handlers</returns>
		/// <exception cref="DomainEventHandlingException">One or more errors occured by handler</exception>
		public async Task HandleAllAsync<TDomainEvent>(TDomainEvent domainEvent)
			where TDomainEvent : IDomainEvent
		{
			var handleTasks = domainEventHandlers
				.Where(HandlesDomainEvent<TDomainEvent>)
				.Select(GetGenericDomainEventHandlerForDomainEvent<TDomainEvent>)
				.Select(handler => HandleAsync(handler, domainEvent))
				.ToList();

			try
			{
				await Task.WhenAll(handleTasks);
			}
			catch (DomainEventHandlingException ex)
			{
				var dehExceptions = handleTasks
					.Where(a => a.Exception != null)
					.SelectMany(a => a.Exception.InnerExceptions)
					.OfType<DomainEventHandlingException>();

				if (dehExceptions.Count() > 1)
					throw new AggregateDomainEventHandlingException(dehExceptions, ex);

				throw ex;
            }
		}

		/// <summary>
		/// Determines if a given domain event handler is mean to handle the domain event. The
		/// handler can be either a exact match or meant to handle a base type of the domain event
		/// </summary>
		/// <typeparam name="TDomainEvent">Type of domain event to be handled</typeparam>
		/// <param name="handler">Domain event handler to be evaluated</param>
		/// <returns></returns>
		private static bool HandlesDomainEvent<TDomainEvent>(IDomainEventHandler handler)
		{
			var expectedDomainEventTypeInfo = typeof(TDomainEvent).GetTypeInfo();
			var handlerInterfaceDomainEventType = GetDomainEventTypeFromHandler(handler);

			if (handlerInterfaceDomainEventType == default(Type))
				return false;

			return handlerInterfaceDomainEventType
				.GetTypeInfo()
				.IsAssignableFrom(expectedDomainEventTypeInfo);
		}

		/// <summary>
		/// Gets the type of domain event that is handled by the domain event handler
		/// </summary>
		/// <param name="handler">Handler</param>
		/// <returns>Type of domain event the handler is meant to handle</returns>
		private static Type GetDomainEventTypeFromHandler(IDomainEventHandler handler)
		{
			var handlerInterface = handler
				.GetType()
				.GetInterfaces()
				.FirstOrDefault(IsGenericDomainEventHandler);

			if (handlerInterface == default(Type))
				return default(Type);

			return handlerInterface.GetGenericArguments()[0];
		}
		
		/// <summary>
		/// Verifies that type of domain event handler implements the generic type of IDomainEventHandler
		/// </summary>
		/// <param name="t">Type of domain event</param>
		/// <returns>True if type implements the the genreic type of IDomainEventHandler</returns>
		private static bool IsGenericDomainEventHandler(Type t)
		{
			var ti = t.GetTypeInfo();

			return ti.IsInterface
				&& ti.IsGenericType
				&& ti.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>);
		}

		/// <summary>
		/// Gets the domain event handler casted to the correct type for the domain event. Wraps the event if needed
		/// </summary>
		/// <typeparam name="TDomainEvent">Type of domain event</typeparam>
		/// <param name="handler">Handler that is known to be correct for the type of domain event</param>
		/// <returns></returns>
		private static IDomainEventHandler<TDomainEvent> GetGenericDomainEventHandlerForDomainEvent<TDomainEvent>(IDomainEventHandler handler)
			where TDomainEvent :  IDomainEvent
		{
			var match = handler as IDomainEventHandler<TDomainEvent>;

			if (match != null)
				return match;

			var baseDomainEventType = GetDomainEventTypeFromHandler(handler);

			var genericDomainEventHandlerWrapper = typeof(DomainEventHandlerWrapper<,>);
			var constructedDomainEventHandlerWrapper = genericDomainEventHandlerWrapper
				.MakeGenericType(baseDomainEventType, typeof(TDomainEvent));

			return (IDomainEventHandler<TDomainEvent>)
				Activator.CreateInstance(constructedDomainEventHandlerWrapper, handler);
		}

		/// <summary>
		/// Handles the domain event. If the domain event throws an exception, a 
		/// DomainEventHandlingException is returned which wraps the original exception.
		/// </summary>
		/// <typeparam name="TDomainEvent">Type of domain event</typeparam>
		/// <param name="handler">Handler</param>
		/// <param name="domainEvent">Domain Event</param>
		/// <returns>A task that represents the the completion of the event being handled.</returns>
		/// <exception cref="DomainEventHandlingException">An error has occured by handler</exception>
		private async Task HandleAsync<TDomainEvent>(IDomainEventHandler<TDomainEvent> handler, TDomainEvent domainEvent)
			where TDomainEvent :  IDomainEvent
		{
			try
			{
				await handler.HandleAsync(domainEvent);
			}
			catch (Exception ex)
			{
				Type handlerType = handler.GetType();

				var wrapper = handler as IDomainEventHandlerWrapper;

				if (wrapper != null)
					handlerType = wrapper.GetBaseHandlerType();

				throw new DomainEventHandlingException(handlerType, domainEvent, ex);
			}
		}

		/// <summary>
		/// Interface used to get the type of the underlying handler within a DomainEventHandlerWrapper
		/// </summary>
		private interface IDomainEventHandlerWrapper
		{
			Type GetBaseHandlerType();
		}

		/// <summary>
		/// When a base domain event handler must be passed in an enumerable, because the generic type does not match,
		/// handlers for base types must be wrapped in an handler of the derived type. This class does this.
		/// </summary>
		/// <typeparam name="TBaseDomainEvent"></typeparam>
		/// <typeparam name="TDerivedDomainEvent"></typeparam>
		private class DomainEventHandlerWrapper<TBaseDomainEvent, TDerivedDomainEvent> : IDomainEventHandler<TDerivedDomainEvent>, IDomainEventHandlerWrapper
			where TBaseDomainEvent : IDomainEvent
			where TDerivedDomainEvent : TBaseDomainEvent
		{
			private readonly IDomainEventHandler<TBaseDomainEvent> domainEventHandler;

			public DomainEventHandlerWrapper(IDomainEventHandler<TBaseDomainEvent> domainEventHandler)
			{
				domainEventHandler.VerifyArgumentNotDefaultValue(nameof(domainEventHandler));

				this.domainEventHandler = domainEventHandler;
			}

			Task IDomainEventHandler<TDerivedDomainEvent>.HandleAsync(TDerivedDomainEvent domainEvent)
			{
				return this.domainEventHandler.HandleAsync(domainEvent);
			}

			Type IDomainEventHandlerWrapper.GetBaseHandlerType()
			{
				return this.domainEventHandler.GetType();
			}
		}
	}
}