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

		public Task HandleAllAsync<TDomainEvent>(TDomainEvent domainEvent) 
			where TDomainEvent : IDomainEvent
		{
			var handleTasks = domainEventHandlers
				.Where(HandlesDomainEvent<TDomainEvent>)
				.Select(GetGenericDomainEventHandlerForDomainEvent<TDomainEvent>)
				.Select(a => a.HandleAsync(domainEvent));

			return Task.WhenAll(handleTasks);
		}

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
		
		private static bool IsGenericDomainEventHandler(Type t)
		{
			var ti = t.GetTypeInfo();

			return ti.IsInterface
				&& ti.IsGenericType
				&& ti.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>);
		}

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

		private class DomainEventHandlerWrapper<TBaseDomainEvent, TDerivedDomainEvent> : IDomainEventHandler<TDerivedDomainEvent>
			where TBaseDomainEvent : IDomainEvent
			where TDerivedDomainEvent : TBaseDomainEvent
		{
			private readonly IDomainEventHandler<TBaseDomainEvent> domainEventHandler;

			public DomainEventHandlerWrapper(IDomainEventHandler<TBaseDomainEvent> domainEventHandler)
			{
				domainEventHandler.VerifyArgumentNotDefaultValue("Domain Event handler is required");

				this.domainEventHandler = domainEventHandler;
			}

			Task IDomainEventHandler<TDerivedDomainEvent>.HandleAsync(TDerivedDomainEvent domainEvent)
			{
				return this.domainEventHandler.HandleAsync(domainEvent);
			}
		}
	}
}
