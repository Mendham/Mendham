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
		private readonly IDomainEventHandlerContainer handlers;
		private readonly IEnumerable<IDomainEventLogger> domainEventLoggers;

		public DomainEventPublisher(IDomainEventHandlerContainer handlers,
			IEnumerable<IDomainEventLogger> domainEventLoggers)
		{
			this.handlers = handlers;
			this.domainEventLoggers = domainEventLoggers;
		}

		public Task RaiseAsync<TDomainEvent>(TDomainEvent domainEvent)
			where TDomainEvent : class, IDomainEvent
		{
			// Log Event
			foreach (var logger in domainEventLoggers)
				logger.LogDomainEvent(domainEvent);

			// Handle Event
			var handlers = GetHandlers(domainEvent)
				.Select(a => a.HandleAsync(domainEvent));

			return Task.WhenAll(handlers);
		}

		private IEnumerable<IDomainEventHandler<TDomainEvent>> GetHandlers<TDomainEvent>(TDomainEvent domainEvent)
			where TDomainEvent : class, IDomainEvent
		{
			throw new NotImplementedException();

			//return handlers.GetHandlers(domainEvent)
			//	.Select(a => a.HandleAsync(domainEvent));


		}

		private static bool IsDomainEventHandler<TDomainEvent>(IDomainEventHandler handler)
			where TDomainEvent : class, IDomainEvent
		{
			var domainEventPassedByParameterType = typeof(TDomainEvent);
			var domainEventFromHandlerInterfaceType = GetDomainEventTypeFromHandler(handler);

			return domainEventFromHandlerInterfaceType
				.GetTypeInfo()
				.IsAssignableFrom(domainEventPassedByParameterType.GetTypeInfo());
		}

		private static Type GetDomainEventTypeFromHandler(IDomainEventHandler handler)
		{
			return handler.GetType()
				.GetInterfaces()
				.First(IsInterfaceThatImplementsDomainHandler)
				.GetGenericArguments()[0];
		}

		private static bool IsInterfaceThatImplementsDomainHandler(Type t)
		{
			var ti = t.GetTypeInfo();

			return ti.IsInterface
				&& ti.IsGenericType
				&& t.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>);
		}

		private static IDomainEventHandler<TDomainEvent> SelectDomainEvents<TDomainEvent>(IDomainEventHandler domainEventHandler)
			where TDomainEvent : class, IDomainEvent
		{
			var exactMatch = domainEventHandler as IDomainEventHandler<TDomainEvent>;

			if (exactMatch != null)
				return exactMatch;

			var baseDomainEventType = GetDomainEventTypeFromHandler(domainEventHandler);

			var genericBaseDomainEventHandlerWrapper = typeof(BaseDomainEventHandlerWrapper<,>);
			var constructedBaseDomainEventHandlerWrapper = genericBaseDomainEventHandlerWrapper
				.MakeGenericType(baseDomainEventType, typeof(TDomainEvent));

			return (IDomainEventHandler<TDomainEvent>)
				Activator.CreateInstance(constructedBaseDomainEventHandlerWrapper, domainEventHandler);
		}

		/// <summary>
		/// When a base domain event handler must be passed in an enumerable, because of problems with contravariance,
		/// handlers for base types must be wrapped in an handler of the derived type. This class does this.
		/// </summary>
		/// <typeparam name="TBaseDomainEvent"></typeparam>
		/// <typeparam name="TDerivedDomainEvent"></typeparam>
		private class BaseDomainEventHandlerWrapper<TBaseDomainEvent, TDerivedDomainEvent> : IDomainEventHandler<TDerivedDomainEvent>
			where TBaseDomainEvent : class, IDomainEvent
			where TDerivedDomainEvent : TBaseDomainEvent, IDomainEvent
		{
			private readonly IDomainEventHandler<TBaseDomainEvent> domainEventHandler;

			public BaseDomainEventHandlerWrapper(IDomainEventHandler<TBaseDomainEvent> domainEventHandler)
			{
				domainEventHandler.VerifyArgumentNotDefaultValue("Domain event handler is required");

				this.domainEventHandler = domainEventHandler;
			}

			Task IDomainEventHandler<TDerivedDomainEvent>.HandleAsync(TDerivedDomainEvent domainEvent)
			{
				return this.domainEventHandler.HandleAsync(domainEvent);
			}
		}
	}
}
