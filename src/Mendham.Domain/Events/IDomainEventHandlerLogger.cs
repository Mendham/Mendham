using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events
{
    public interface IDomainEventHandlerLogger
    {
        /// <summary>
        /// Logs when a domain event handler is about to be invoked
        /// </summary>
        /// <param name="handlerType">Handler about to be involved</param>
        /// <param name="domainEvent">Domain event being handled</param>
        void LogDomainEventHandlerStarting(Type handlerType, IDomainEvent domainEvent);

        /// <summary>
        /// Logs when a domain event handler is successfully invoked
        /// </summary>
        /// <param name="handlerType">Handler that was involved</param>
        /// <param name="domainEvent">Domain event handled</param>
        void LogDomainEventHandlerComplete(Type handlerType, IDomainEvent domainEvent);

        /// <summary>
        /// Logs when a domain event handler throws an error
        /// </summary>
        /// <param name="handlerType">Handler that was involved</param>
        /// <param name="domainEvent">Domain event handled</param>
        void LogDomainEventHandlerError(Type handlerType, IDomainEvent domainEvent, Exception exception);
    }
}
