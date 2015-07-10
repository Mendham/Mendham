using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Mendham;

namespace Mendham.Domain.Events
{
    public class DomainEventHandlingException : Exception
    {
		public IDomainEvent DomainEvent { get; protected set; }
		public Type DomainEventHandlerType { get; protected set; }

		private const string DEFAULT_MESSAGE = "An exception occured when handling the domain event";

		protected DomainEventHandlingException(DomainEventHandlingException firstException)
			:base(DEFAULT_MESSAGE, firstException)
		{ }

		internal DomainEventHandlingException(Type domainEventHandler, IDomainEvent domainEvent, Exception exception)
			:base(DEFAULT_MESSAGE, exception)
		{
			domainEvent.VerifyArgumentNotDefaultValue("Domain event is required");
			domainEventHandler.VerifyArgumentNotDefaultValue("Domain event handler type is required");

			this.DomainEvent = domainEvent;
			this.DomainEventHandlerType = domainEventHandler;
		}

		public override string Message
		{
			get
			{
				return string.Format("{0} {1}. See INNER EXCEPTION for details",
					DEFAULT_MESSAGE,
					DomainEventHandlerType.FullName);
			}
		}
	}

	[DebuggerDisplay("Count = {Count}")]
	public class AggregateDomainEventHandlingException : DomainEventHandlingException
	{
		private readonly IReadOnlyCollection<DomainEventHandlingException> domainEventHandlingExceptions;

		internal AggregateDomainEventHandlingException(IEnumerable<DomainEventHandlingException> domainEventHandlingExceptions, DomainEventHandlingException firstException)
			:base(firstException)
		{
			domainEventHandlingExceptions
				.VerifyArgumentNotNullOrEmpty("The exceptions passed cannot be null or empty")
				.VerifyArgumentMeetsCriteria(a => a.Count() > 1, "AggregateDomainEventHandlingException more than one exception")
				.VerifyArgumentMeetsCriteria(a => a
					.Select(b => b.DomainEvent)
					.Distinct()
					.Count() == 1, "The exceptions passed do not all have a matching domain event");

			this.DomainEvent = firstException.DomainEvent;
			this.DomainEventHandlerType = firstException.DomainEventHandlerType;

#if DNXCORE50
			// Not sure why this AsReadOnly isn't available for DNXCORE50
			this.domainEventHandlingExceptions = new ReadOnlyCollection<DomainEventHandlingException>(domainEventHandlingExceptions.ToList());
#else
			this.domainEventHandlingExceptions = domainEventHandlingExceptions.ToList().AsReadOnly();
#endif
		}

		public IEnumerable<Type> DomainEventHandlerTypes
		{
			get
			{
				return domainEventHandlingExceptions
					.Select(a => a.DomainEventHandlerType);
			}
		}

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
