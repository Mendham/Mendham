using Mendham.Domain.Events;
using Mendham.Domain.Events.Components;
using Mendham.Domain.Test.TestObjects.Events;
using Mendham.Testing;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Mendham.Domain.Test.Fixtures
{
    public class DomainEventPublisherFixture : Fixture<DomainEventPublisher>
    {
		public IDomainEventHandlerContainer DomainEventHandlerContainer { get; set; }
        public IDomainEventHandlerProcessor DomainEventHandlerProcessor { get; set; }
        public IDomainEventLoggerProcessor DomainEventLoggerProcessor { get; set; }

        public override DomainEventPublisher CreateSut()
		{
			return new DomainEventPublisher(DomainEventPublisherContainerFactory);
		}

		public override void ResetFixture()
		{
			base.ResetFixture();

			this.DomainEventHandlerContainer = Mock.Of<IDomainEventHandlerContainer>();
            this.DomainEventHandlerProcessor = Mock.Of<IDomainEventHandlerProcessor>();
            this.DomainEventLoggerProcessor = Mock.Of<IDomainEventLoggerProcessor>();
        }

        public TestDomainEvent CreateDomainEvent()
        {
            return new TestDomainEvent();
        }

        public IEnumerable<IDomainEventHandler<TestDomainEvent>> GetDomainEventHandlersForTestDomainEvent()
        {
            return Mock.Of<IEnumerable<IDomainEventHandler<TestDomainEvent>>>();
        }

        private IDomainEventPublisherComponents DomainEventPublisherContainerFactory()
        {
            return new DomainEventPublisherComponents(DomainEventHandlerContainer, 
                DomainEventHandlerProcessor, DomainEventLoggerProcessor);
        }
	}
}
