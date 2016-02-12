using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events
{
    public class DomainEventPublisherComponents : IDomainEventPublisherComponents
    {
        private readonly IDomainEventHandlerContainer handlerContainer;
        private readonly IEnumerable<IDomainEventLogger> loggers;

        public DomainEventPublisherComponents(IDomainEventHandlerContainer handlerContainer,
            IEnumerable<IDomainEventLogger> loggers)
        {
            this.handlerContainer = handlerContainer;
            this.loggers = loggers.ToList();
        }

        public IDomainEventHandlerContainer DomainEventHandlerContainer
        {
            get
            {
                return handlerContainer;
            }
        }

        public IEnumerable<IDomainEventLogger> DomainEventLoggers
        {
            get
            {
                return loggers;
            }
        }
    }
}
