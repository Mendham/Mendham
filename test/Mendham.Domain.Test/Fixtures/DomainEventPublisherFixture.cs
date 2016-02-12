using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham.Domain.Events;
using Mendham.Testing;
using Moq;

namespace Mendham.Domain.Test.Fixtures
{
    public class DomainEventPublisherFixture : Fixture<DomainEventPublisher>
    {
		public IDomainEventHandlerContainer DomainEventHandlerContainer { get; set; }
		public IDomainEventLoggerContainer DomainEventLoggerContainer { get; set; }

		public override DomainEventPublisher CreateSut()
		{
			return new DomainEventPublisher(DomainEventPublisherContainerFactory);
		}

		public override void ResetFixture()
		{
			base.ResetFixture();

			this.DomainEventHandlerContainer = Mock.Of<IDomainEventHandlerContainer>();
			this.DomainEventLoggerContainer = Mock.Of<IDomainEventLoggerContainer>();
        }

		public class TestDomainEvent : DomainEvent
		{ }

		public TestDomainEvent CreateDomainEvent()
		{
			return new TestDomainEvent();
		}

        private IDomainEventPublisherComponents DomainEventPublisherContainerFactory()
        {
            return new DomainEventPublisherComponents(DomainEventHandlerContainer, DomainEventLoggerContainer);
        }
	}
}
