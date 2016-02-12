using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events
{
    public class DomainEventPublisherComponents : IDomainEventPublisherComponents
    {
        private readonly IDomainEventHandlerContainer handlerContainer;
        private readonly IDomainEventLoggerContainer loggerContainer;

        public DomainEventPublisherComponents(IDomainEventHandlerContainer handlerContainer,
            IDomainEventLoggerContainer loggerContainer)
        {
            this.handlerContainer = handlerContainer;
            this.loggerContainer = loggerContainer;
        }

        public IDomainEventHandlerContainer DomainEventHandlerContainer
        {
            get
            {
                return handlerContainer;
            }
        }

        public IDomainEventLoggerContainer DomainEventLoggerContainer
        {
            get
            {
                return loggerContainer;
            }
        }
    }
}
