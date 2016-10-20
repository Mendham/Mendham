using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mendham.Events.Components
{
    public static class EventHandlerContainerExtensions
    {
        /// <summary>
        /// Given a set of <see cref="IEventHandler"/>, select all that can handle the type of event (include base types)
        /// </summary>
        /// <typeparam name="TEvent">Type of event to process</typeparam>
        /// <param name="allHandlers">All known handlers</param>
        /// <returns>All handlers that can process the event</returns>
        public static IEnumerable<IEventHandler<TEvent>> SelectHandlersForEvent<TEvent>(this IEnumerable<IEventHandler> allHandlers)
            where TEvent : IEvent
        {
            return allHandlers
                .Where(HandlesEvent<TEvent>)
                .Select(GetGenericEventHandlerForEvent<TEvent>);
        }

        /// <summary>
		/// Determines if a given event handler is mean to handle the event. The
		/// handler can be either a exact match or meant to handle a base type of the event
		/// </summary>
		/// <typeparam name="TEvent">Type of event to be handled</typeparam>
		/// <param name="handler">Event handler to be evaluated</param>
		/// <returns></returns>
		private static bool HandlesEvent<TEvent>(IEventHandler handler)
        {
            var expectedEventTypeInfo = typeof(TEvent);
            var handlerInterfaceEventTypes = GetEventTypesFromHandler(handler);

            return handlerInterfaceEventTypes
                .Any(a => a.GetTypeInfo().IsAssignableFrom(expectedEventTypeInfo.GetTypeInfo()));
        }

        /// <summary>
		/// Gets the type of event that is handled by the event handler
		/// </summary>
		/// <param name="handler">Handler</param>
		/// <returns>Types of events the handler is meant to handle</returns>
		private static IEnumerable<Type> GetEventTypesFromHandler(IEventHandler handler)
        {
            return handler
                .GetType()
                .GetInterfaces()
                .Where(IsGenericEventHandler)
                .Select(a => a.GetGenericArguments()[0]);
        }

        /// <summary>
		/// Gets the event handler casted to the correct type for the event. Wraps the event if needed
		/// </summary>
		/// <typeparam name="TEvent">Type of event</typeparam>
		/// <param name="handler">Handler that is known to be correct for the type of event</param>
		/// <returns></returns>
		private static IEventHandler<TEvent> GetGenericEventHandlerForEvent<TEvent>(IEventHandler handler)
            where TEvent : IEvent
        {
            var match = handler as IEventHandler<TEvent>;

            if (match != null)
                return match;

            Type baseEventType = GetEventTypesFromHandler(handler)
                .FirstOrDefault(a => a.GetTypeInfo().IsAssignableFrom(typeof(TEvent).GetTypeInfo()));

            var genericEventHandlerWrapper = typeof(EventHandlerWrapper<,>);
            var constructedEventHandlerWrapper = genericEventHandlerWrapper
                .MakeGenericType(baseEventType, typeof(TEvent));

            return (IEventHandler<TEvent>)
                Activator.CreateInstance(constructedEventHandlerWrapper, handler);
        }

        /// <summary>
		/// Verifies that type of event handler implements the generic type of <see cref="IEventHandler"/>
		/// </summary>
		/// <param name="t">Type of event</param>
		/// <returns>True if type implements the the genreic type of <see cref="IEventHandler"/></returns>
		private static bool IsGenericEventHandler(Type t)
        {
            var ti = t.GetTypeInfo();

            return ti.IsInterface
                && ti.IsGenericType
                && ti.GetGenericTypeDefinition().Equals(typeof(IEventHandler<>));
        }
    }
}
