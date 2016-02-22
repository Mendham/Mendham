using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Domain.Events.Components
{
    public static class DomainEventHandlerContainerExtensions
    {
        /// <summary>
        /// Given a set of IDomainEventHandlers, select all that can handle the type of domain event (include base types)
        /// </summary>
        /// <typeparam name="TDomainEvent">Type of domain event to process</typeparam>
        /// <param name="allHandlers">All known handlers</param>
        /// <returns>All handlers that can process the domain event</returns>
        public static IEnumerable<IDomainEventHandler<TDomainEvent>> SelectHandlersForDomainEvent<TDomainEvent>(this IEnumerable<IDomainEventHandler> allHandlers)
            where TDomainEvent : IDomainEvent
        {
            return allHandlers
                .Where(HandlesDomainEvent<TDomainEvent>)
                .Select(GetGenericDomainEventHandlerForDomainEvent<TDomainEvent>);
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
            var expectedDomainEventTypeInfo = typeof(TDomainEvent);
            var handlerInterfaceDomainEventType = GetDomainEventTypeFromHandler(handler);

            if (handlerInterfaceDomainEventType == default(Type))
                return false;

            return handlerInterfaceDomainEventType
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
		/// Gets the domain event handler casted to the correct type for the domain event. Wraps the event if needed
		/// </summary>
		/// <typeparam name="TDomainEvent">Type of domain event</typeparam>
		/// <param name="handler">Handler that is known to be correct for the type of domain event</param>
		/// <returns></returns>
		private static IDomainEventHandler<TDomainEvent> GetGenericDomainEventHandlerForDomainEvent<TDomainEvent>(IDomainEventHandler handler)
            where TDomainEvent : IDomainEvent
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
		/// Verifies that type of domain event handler implements the generic type of IDomainEventHandler
		/// </summary>
		/// <param name="t">Type of domain event</param>
		/// <returns>True if type implements the the genreic type of IDomainEventHandler</returns>
		private static bool IsGenericDomainEventHandler(Type t)
        {
            var ti = t.GetTypeInfo();

            return ti.IsInterface
                && ti.IsGenericType
                && ti.GetGenericTypeDefinition().Equals(typeof(IDomainEventHandler<>));
        }
    }
}
