using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham;

namespace Mendham.Domain.Events
{
    public class DomainEventHandlingException : Exception
    {
		public IDomainEvent DomainEvent { get; private set; }
		public Type DomainEventHandlerType { get; private set; }

		private const string DEFAULT_MESSAGE = "An exception occured when handling the domain event";

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
}
