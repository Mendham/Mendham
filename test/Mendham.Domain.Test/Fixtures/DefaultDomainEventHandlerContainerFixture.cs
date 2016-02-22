using Mendham.Domain.Events;
using Mendham.Domain.Events.Components;
using Mendham.Domain.Test.TestObjects.Events;
using Mendham.Testing;
using Moq;
using System.Collections.Generic;

namespace Mendham.Domain.Test.Fixtures
{
    public class DefaultDomainEventHandlerContainerFixture : Fixture<DefaultDomainEventHandlerContainer>
    {
		public IDomainEventHandler<BaseDomainEvent> BaseEventHandler { get; set; }
		public IDomainEventHandler<DerivedDomainEvent> DerivedEventHandler { get; set; }
		public IDomainEventHandler<OtherDomainEvent> OtherEventHandler { get; set; }

        public override DefaultDomainEventHandlerContainer CreateSut()
		{
			return new DefaultDomainEventHandlerContainer(() => Handlers);
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
		}

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
