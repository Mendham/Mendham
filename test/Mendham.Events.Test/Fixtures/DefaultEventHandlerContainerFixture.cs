using Mendham.Events.Components;
using Mendham.Events.Test.TestObjects;
using Mendham.Testing;
using Moq;
using System.Collections.Generic;

namespace Mendham.Events.Test.Fixtures
{
    public class DefaultEventHandlerContainerFixture : Fixture<DefaultEventHandlerContainer>
    {
		public IEventHandler<BaseEvent> BaseEventHandler { get; set; }
		public IEventHandler<DerivedEvent> DerivedEventHandler { get; set; }
		public IEventHandler<OtherEvent> OtherEventHandler { get; set; }

        public override DefaultEventHandlerContainer CreateSut()
		{
			return new DefaultEventHandlerContainer(() => Handlers);
		}

		private IEnumerable<IEventHandler> Handlers
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

			BaseEventHandler = Mock.Of<IEventHandler<BaseEvent>>();
			DerivedEventHandler = Mock.Of<IEventHandler<DerivedEvent>>();
			OtherEventHandler = Mock.Of<IEventHandler<OtherEvent>>();
		}

		public BaseEvent CreateBaseEvent()
		{
			return new BaseEvent();
		}

		public DerivedEvent CreateDerivedEvent()
		{
			return new DerivedEvent();
		}
	}
}
