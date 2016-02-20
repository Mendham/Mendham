using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Mendham.Domain.Events
{
    /// <summary>
	/// An exception that contains details when multiple domain event handlers throw an excpetion
	/// during the processing of a single domain event being raised.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
    public class AggregateDomainEventHandlingException : DomainEventHandlingException
    {
        private readonly IReadOnlyCollection<DomainEventHandlingException> domainEventHandlingExceptions;

        internal AggregateDomainEventHandlingException(IEnumerable<DomainEventHandlingException> domainEventHandlingExceptions, DomainEventHandlingException firstException)
            : base(firstException)
        {
            domainEventHandlingExceptions
                .VerifyArgumentNotNullOrEmpty(nameof(domainEventHandlingExceptions), "The exceptions passed cannot be null or empty")
                .VerifyArgumentMeetsCriteria(a => a.Count() > 1,
                nameof(domainEventHandlingExceptions), "AggregateDomainEventHandlingException more than one exception")
                .VerifyArgumentMeetsCriteria(a => a
                    .Select(b => b.DomainEvent)
                    .Distinct()
                    .Count() == 1, nameof(domainEventHandlingExceptions), "The exceptions passed do not all have a matching domain event");

            this.DomainEvent = firstException.DomainEvent;
            this.DomainEventHandlerType = firstException.DomainEventHandlerType;

            this.domainEventHandlingExceptions = new ReadOnlyCollection<DomainEventHandlingException>(domainEventHandlingExceptions.ToList());
        }

        /// <summary>
        /// Types of the domain event handlers that threw an exception
        /// </summary>
        public IEnumerable<Type> DomainEventHandlerTypes
        {
            get
            {
                return domainEventHandlingExceptions
                    .Select(a => a.DomainEventHandlerType);
            }
        }

        /// <summary>
        /// Exceptions thrown by domain event handlers
        /// </summary>
        public IEnumerable<DomainEventHandlingException> InnerExceptions
        {
            get
            {
                return domainEventHandlingExceptions;
            }
        }

        public override string Message
        {
            get
            {
                return string.Format("Multiple exceptions occured when handling the domain event. Exception count {0}. See See INNER EXCEPTIONS for details.",
                    this.domainEventHandlingExceptions.Count());
            }
        }
    }
}
