using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Mendham.Events
{
    /// <summary>
	/// An exception that contains details when multiple event handlers throw an excpetion
	/// during the processing of a single event being raised.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
    public class AggregateEventHandlingException : EventHandlingException
    {
        private readonly IReadOnlyCollection<EventHandlingException> _eventHandlingExceptions;

        internal AggregateEventHandlingException(IEnumerable<EventHandlingException> eventHandlingExceptions, EventHandlingException firstException)
            : base(firstException)
        {
            eventHandlingExceptions
                .VerifyArgumentNotNullOrEmpty(nameof(eventHandlingExceptions), "The exceptions passed cannot be null or empty")
                .VerifyArgumentMeetsCriteria(a => a.Count() > 1,
                    nameof(eventHandlingExceptions), $"{nameof(AggregateEventHandlingException)} requires more than one exception")
                .VerifyArgumentMeetsCriteria(a => a
                    .Select(b => b.Event)
                    .Distinct()
                    .Count() == 1, nameof(eventHandlingExceptions), "The exceptions passed do not all have a matching event");

            _eventHandlingExceptions = new ReadOnlyCollection<EventHandlingException>(eventHandlingExceptions.ToList());
        }

        /// <summary>
        /// Types of the event handlers that threw an exception
        /// </summary>
        public IEnumerable<Type> EventHandlerTypes
        {
            get
            {
                return _eventHandlingExceptions
                    .Select(a => a.EventHandlerType);
            }
        }

        /// <summary>
        /// Exceptions thrown by event handlers
        /// </summary>
        public IEnumerable<EventHandlingException> InnerExceptions
        {
            get
            {
                return _eventHandlingExceptions;
            }
        }

        public override string Message
        {
            get
            {
                return string.Format("Multiple exceptions occured when handling the event. Exception count {0}. See See INNER EXCEPTIONS for details.",
                    _eventHandlingExceptions.Count());
            }
        }
    }
}
