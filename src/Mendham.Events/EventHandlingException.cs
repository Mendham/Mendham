using System;

namespace Mendham.Events
{
    /// <summary>
    /// Exception that occurs when an event handler throws an exception when an event is rasied.
    /// The inner exception contains the details of the exception.
    /// </summary>
    public class EventHandlingException : Exception
    {
        /// <summary>
		/// The event raised when the exception occured
		/// </summary>
        public IEvent Event { get; }

        /// <summary>
		/// The type of the handler that threw an exception
		/// </summary>
		public Type EventHandlerType { get; }

        private const string DEFAULT_MESSAGE = "An exception occured when handling the domain event";

        protected EventHandlingException(EventHandlingException firstException)
            : this(firstException.EventHandlerType, firstException.Event, firstException)
        { }

        internal EventHandlingException(Type eventHandler, IEvent eventRaised, Exception exception)
            : base(DEFAULT_MESSAGE, exception)
        {
            Event = eventRaised
                .VerifyArgumentNotDefaultValue(nameof(eventRaised));
            EventHandlerType = eventHandler
                .VerifyArgumentNotDefaultValue(nameof(eventHandler));
        }

        public override string Message
        {
            get
            {
                return $"An exception in handler {EventHandlerType.FullName} occured when processing a '{Event.GetType().FullName}' event. See INNER EXCEPTION for details";
            }
        }
    }
}
