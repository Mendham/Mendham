using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events.Components
{
    public class DomainEventHandlerProcessor : IDomainEventHandlerProcessor
    {
        private readonly IEnumerable<IDomainEventHandlerLogger> domainEventHandlerLoggers;

        public DomainEventHandlerProcessor(IEnumerable<IDomainEventHandlerLogger> domainEventHandlerLoggers)
        {
            this.domainEventHandlerLoggers = domainEventHandlerLoggers;
        }

        public async Task HandleAllAsync<TDomainEvent>(TDomainEvent domainEvent, IEnumerable<IDomainEventHandler<TDomainEvent>> handlers) 
            where TDomainEvent : IDomainEvent
        {
            // Start all handlers and return tasks
            var handlerTasks = handlers
                .Select(handler => HandleAsync(handler, domainEvent))
                .ToList();

            try
            {
                await Task.WhenAll(handlerTasks);
            }
            catch (DomainEventHandlingException ex)
            {
                var dehExceptions = handlerTasks
                    .Where(a => a.Exception != null)
                    .SelectMany(a => a.Exception.InnerExceptions)
                    .OfType<DomainEventHandlingException>();

                if (dehExceptions.Count() > 1)
                    throw new AggregateDomainEventHandlingException(dehExceptions, ex);

                throw ex;
            }
        }

        /// <summary>
		/// Handles the domain event. If the domain event throws an exception, a 
		/// DomainEventHandlingException is returned which wraps the original exception.
		/// </summary>
		/// <typeparam name="TDomainEvent">Type of domain event</typeparam>
		/// <param name="handler">Handler</param>
		/// <param name="domainEvent">Domain Event</param>
		/// <returns>A task that represents the the completion of the event being handled.</returns>
		/// <exception cref="DomainEventHandlingException">An error has occured by handler</exception>
		private async Task HandleAsync<TDomainEvent>(IDomainEventHandler<TDomainEvent> handler, TDomainEvent domainEvent)
            where TDomainEvent : IDomainEvent
        {
            Type handlerType = GetHandlerType(handler);

            try
            {
                WriteToDomainEventHandlerLogger(a => a.LogDomainEventHandlerStart(handlerType, domainEvent));

                await handler.HandleAsync(domainEvent);

                WriteToDomainEventHandlerLogger(a => a.LogDomainEventHandlerComplete(handlerType, domainEvent));
            }
            catch (Exception ex)
            {
                WriteToDomainEventHandlerLogger(a => a.LogDomainEventHandlerError(handlerType, domainEvent, ex));

                throw new DomainEventHandlingException(handlerType, domainEvent, ex);
            }
        }

        private void WriteToDomainEventHandlerLogger(Action<IDomainEventHandlerLogger> writeAction)
        {
            foreach (var domainEventHandlerLogger in domainEventHandlerLoggers)
            {
                writeAction(domainEventHandlerLogger);
            }
        }

        private static Type GetHandlerType(IDomainEventHandler handler)
        {
            Type handlerType = handler.GetType();

            var wrapper = handler as IDomainEventHandlerWrapper;

            if (wrapper != null)
            {
                handlerType = wrapper.GetBaseHandlerType();
            }

            return handlerType;
        }
    }
}
