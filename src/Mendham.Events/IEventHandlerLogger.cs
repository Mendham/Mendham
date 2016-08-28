using System;

namespace Mendham.Events
{
    /// <summary>
    /// Logs the progress of each event handler that handles a given event
    /// </summary>
    public interface IEventHandlerLogger
    {
        /// <summary>
        /// Logs when a event handler is about to be invoked
        /// </summary>
        /// <param name="handlerType">Handler about to be involved</param>
        /// <param name="eventRaised">Event about to be handled</param>
        void LogEventHandlerStart(Type handlerType, IEvent eventRaised);

        /// <summary>
        /// Logs when a event handler is successfully invoked
        /// </summary>
        /// <param name="handlerType">Handler that was involved</param>
        /// <param name="eventHandled">Event handled</param>
        void LogEventHandlerComplete(Type handlerType, IEvent eventHandled);

        /// <summary>
        /// Logs when a event handler throws an error
        /// </summary>
        /// <param name="handlerType">Handler that was involved</param>
        /// <param name="eventHandled">Event handled</param>
        void LogEventHandlerError(Type handlerType, IEvent eventHandled, Exception exception);
    }
}
