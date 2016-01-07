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
		public IEnumerable<IDomainEventLogger> DomainEventLoggers { get; set; }

		public override DomainEventPublisher CreateSut()
		{
			return new DomainEventPublisher(this.DomainEventHandlerContainer,
				this.DomainEventLoggers);
		}

		public override void ResetFixture()
		{
			base.ResetFixture();

			this.DomainEventHandlerContainer = Mock.Of<IDomainEventHandlerContainer>();
			this.DomainEventLoggers = Enumerable.Empty<IDomainEventLogger>();
		}

		public class TestDomainEvent : DomainEvent
		{ }

		public TestDomainEvent CreateDomainEvent()
		{
			return new TestDomainEvent();
		}
	}
}
