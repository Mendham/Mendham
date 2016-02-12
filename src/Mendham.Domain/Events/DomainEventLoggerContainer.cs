using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events
{
    public class DomainEventLoggerContainer : IDomainEventLoggerContainer
    {
        private readonly IEnumerable<IDomainEventLogger> domainEventLoggers;

        public DomainEventLoggerContainer(IEnumerable<IDomainEventLogger> domainEventLoggers)
        {
            this.domainEventLoggers = domainEventLoggers;
        }

        /// <summary>
        /// Writes to all registered loggers
        /// </summary>
		/// <typeparam name="TDomainEvent">Type of domain event</typeparam>
		/// <param name="domainEvent">Domain Event</param>
        public void WriteToAllLoggers<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent
        {
            foreach (var logger in domainEventLoggers)
            {
                logger.LogDomainEvent(domainEvent);
            }
        }
    }
}
