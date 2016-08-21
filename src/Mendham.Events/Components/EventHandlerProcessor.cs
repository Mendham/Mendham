using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Events.Components
{
    public class EventHandlerProcessor : IEventHandlerProcessor
    {
        private readonly IEnumerable<IEventHandlerLogger> _eventHandlerLoggers;

        public EventHandlerProcessor(IEnumerable<IEventHandlerLogger> eventHandlerLoggers)
        {
            _eventHandlerLoggers = eventHandlerLoggers;
        }

        public async Task HandleAllAsync<TEvent>(TEvent eventRaised, IEnumerable<IEventHandler<TEvent>> handlers) 
            where TEvent : IEvent
        {
            // Start all handlers and return tasks
            var handlerTasks = handlers
                .Select(handler => HandleAsync(handler, eventRaised))
                .ToList();

            try
            {
                await Task.WhenAll(handlerTasks);
            }
            catch (EventHandlingException ex)
            {
                var dehExceptions = handlerTasks
                    .Where(a => a.Exception != null)
                    .SelectMany(a => a.Exception.InnerExceptions)
                    .OfType<EventHandlingException>();

                if (dehExceptions.Count() > 1)
                    throw new AggregateEventHandlingException(dehExceptions, ex);

                throw ex;
            }
        }

        /// <summary>
		/// Handles the main event. If the event throws an exception, a 
		/// <see cref="EventHandlingException"/> is returned which wraps the original exception.
		/// </summary>
		/// <typeparam name="TEvent">Type of event</typeparam>
		/// <param name="handler">Handler</param>
		/// <param name="eventRaised">Event raised</param>
		/// <returns>A task that represents the the completion of the event being handled.</returns>
		/// <exception cref="EventHandlingException">An error has occured by handler</exception>
		private async Task HandleAsync<TEvent>(IEventHandler<TEvent> handler, TEvent eventRaised)
            where TEvent : IEvent
        {
            Type handlerType = GetHandlerType(handler);

            try
            {
                WriteToEventHandlerLogger(a => a.LogEventHandlerStart(handlerType, eventRaised));

                await handler.HandleAsync(eventRaised);

                WriteToEventHandlerLogger(a => a.LogEventHandlerComplete(handlerType, eventRaised));
            }
            catch (Exception ex)
            {
                WriteToEventHandlerLogger(a => a.LogEventHandlerError(handlerType, eventRaised, ex));

                throw new EventHandlingException(handlerType, eventRaised, ex);
            }
        }

        private void WriteToEventHandlerLogger(Action<IEventHandlerLogger> writeAction)
        {
            foreach (var eventHandlerLogger in _eventHandlerLoggers)
            {
                writeAction(eventHandlerLogger);
            }
        }

        private static Type GetHandlerType(IEventHandler handler)
        {
            Type handlerType = handler.GetType();

            var wrapper = handler as IEventHandlerWrapper;

            if (wrapper != null)
            {
                handlerType = wrapper.GetBaseHandlerType();
            }

            return handlerType;
        }
    }
}
