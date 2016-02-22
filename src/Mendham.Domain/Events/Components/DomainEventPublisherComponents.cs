using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events.Components
{
    public class DomainEventPublisherComponents : IDomainEventPublisherComponents
    {
        private readonly IDomainEventHandlerContainer domainEventHandlerContainer;
        private readonly IDomainEventHandlerProcessor domainEventHandlerProcessor;
        private readonly IDomainEventLoggerProcessor domainEventLoggerProcessor;

        public DomainEventPublisherComponents(IDomainEventHandlerContainer domainEventHandlerContainer,
            IDomainEventHandlerProcessor domainEventHandlerProcessor,
            IDomainEventLoggerProcessor domainEventLoggerProcessor)
        {
            this.domainEventHandlerContainer = domainEventHandlerContainer;
            this.domainEventHandlerProcessor = domainEventHandlerProcessor;
            this.domainEventLoggerProcessor = domainEventLoggerProcessor;
        }

        public IDomainEventHandlerContainer DomainEventHandlerContainer
        {
            get
            {
                return domainEventHandlerContainer;
            }
        }

        public IDomainEventHandlerProcessor DomainEventHandlerProcessor
        {
            get
            {
                return domainEventHandlerProcessor;
            }
        }

        public IDomainEventLoggerProcessor DomainEventLoggerProcessor
        {
            get
            {
                return domainEventLoggerProcessor;
            }
        }
    }
}
