using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events.Components
{
    public class DomainEventLoggerProcessor : IDomainEventLoggerProcessor
    {
        private readonly Func<IEnumerable<IDomainEventLogger>> loggersFactory;

        public DomainEventLoggerProcessor(Func<IEnumerable<IDomainEventLogger>> loggersFactory)
        {
            this.loggersFactory = loggersFactory;
        }

        public void LogDomainEvent(IDomainEvent domainEvent)
        {
            foreach (var logger in loggersFactory())
            {
                logger.LogDomainEventRaised(domainEvent);
            }
        }
    }
}
