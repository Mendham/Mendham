using Mendham.Domain.Events;
using Mendham.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.Fixtures
{
    public class DomainEventHandlerContainerFixture : Fixture<DomainEventHandlerContainer>
    {

		public IDomainEventHandler<BaseDomainEvent> BaseEventHandler { get; set; }
		public IDomainEventHandler<DerivedDomainEvent> DerivedEventHandler { get; set; }
		public IDomainEventHandler<OtherDomainEvent> OtherEventHandler { get; set; }

        public IDomainEventHandlerLogger DomainEventHandlerLogger { get; set; }

        public override DomainEventHandlerContainer CreateSut()
		{
			return new DomainEventHandlerContainer(Handlers, DomainEventHandlerLogger.AsSingleItemEnumerable());
		}

		private IEnumerable<IDomainEventHandler> Handlers
		{
			get
			{
				yield return BaseEventHandler;
				yield return DerivedEventHandler;
				yield return OtherEventHandler;
			}
		}

		public override void ResetFixture()
		{
			base.ResetFixture();

			BaseEventHandler = Mock.Of<IDomainEventHandler<BaseDomainEvent>>();
			DerivedEventHandler = Mock.Of<IDomainEventHandler<DerivedDomainEvent>>();
			OtherEventHandler = Mock.Of<IDomainEventHandler<OtherDomainEvent>>();

            DomainEventHandlerLogger = Mock.Of<IDomainEventHandlerLogger>();
		}

		public class BaseDomainEvent : DomainEvent
		{ }

		public class DerivedDomainEvent : BaseDomainEvent
		{ }

		public class OtherDomainEvent : DomainEvent
		{ }

		public BaseDomainEvent CreateBaseDomainEvent()
		{
			return new BaseDomainEvent();
		}

		public DerivedDomainEvent CreateDerivedDomainEvent()
		{
			return new DerivedDomainEvent();
		}
	}
}
