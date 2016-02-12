using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events
{
    public interface IDomainEventLoggerContainer
    {
        /// <summary>
        /// Writes a log message to all loggers registered with the container
        /// </summary>
		/// <typeparam name="TDomainEvent">Type of domain event</typeparam>
        /// <param name="domainEvent">Domain Event</param>
        void WriteToAllLoggers<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : IDomainEvent;
    }
}
