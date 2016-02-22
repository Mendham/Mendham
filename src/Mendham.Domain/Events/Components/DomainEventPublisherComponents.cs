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
        private readonly IEnumerable<IDomainEventLogger> domainEventLoggers;

        public DomainEventPublisherComponents(IDomainEventHandlerContainer domainEventHandlerContainer,
            IDomainEventHandlerProcessor domainEventHandlerProcessor,
            IEnumerable<IDomainEventLogger> domainEventLoggers)
        {
            this.domainEventHandlerContainer = domainEventHandlerContainer;
            this.domainEventHandlerProcessor = domainEventHandlerProcessor;
            this.domainEventLoggers = domainEventLoggers;
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

        public IEnumerable<IDomainEventLogger> DomainEventLoggers
        {
            get
            {
                return domainEventLoggers;
            }
        }
    }
}
