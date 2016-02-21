using System;

namespace Mendham.Domain.Events
{
    /// <summary>
    /// Exception that occurs when an event handler throws an exception when an event is rasied.
    /// The inner exception contains the details of the exception.
    /// </summary>
    public class DomainEventHandlingException : Exception
    {
		/// <summary>
		/// The domain event raised when the exception occured
		/// </summary>
		public IDomainEvent DomainEvent { get; protected set; }
		/// <summary>
		/// The type of the handler that threw an exception
		/// </summary>
		public Type DomainEventHandlerType { get; protected set; }

		private const string DEFAULT_MESSAGE = "An exception occured when handling the domain event";

		protected DomainEventHandlingException(DomainEventHandlingException firstException)
			:base(DEFAULT_MESSAGE, firstException)
		{ }

		internal DomainEventHandlingException(Type domainEventHandler, IDomainEvent domainEvent, Exception exception)
			:base(DEFAULT_MESSAGE, exception)
		{
			domainEvent.VerifyArgumentNotDefaultValue(nameof(domainEvent));
			domainEventHandler.VerifyArgumentNotDefaultValue(nameof(domainEventHandler));

			this.DomainEvent = domainEvent;
			this.DomainEventHandlerType = domainEventHandler;
		}

		public override string Message
		{
			get
			{
                return $"An exception in handler {DomainEventHandlerType.FullName} occured when processing a '{DomainEvent.GetType().FullName}' domain event. See INNER EXCEPTION for details";
			}
		}
	}
}
