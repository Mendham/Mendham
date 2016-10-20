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

        public IEventHandler SharedEventHandler { get; set; }

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
                yield return SharedEventHandler;
            }
		}

		public override void ResetFixture()
		{
			base.ResetFixture();

			BaseEventHandler = Mock.Of<IEventHandler<BaseEvent>>();
			DerivedEventHandler = Mock.Of<IEventHandler<DerivedEvent>>();
			OtherEventHandler = Mock.Of<IEventHandler<OtherEvent>>();

            var sharedEventHandleMock = new Mock<IEventHandler<SharedEvent1>>();
            SharedEventHandler = sharedEventHandleMock.As<IEventHandler<SharedEvent2>>().Object;
		}

		public BaseEvent CreateBaseEvent()
		{
			return new BaseEvent();
		}

		public DerivedEvent CreateDerivedEvent()
		{
			return new DerivedEvent();
		}

        public SharedEvent1 CreateSharedEvent1()
        {
            return new SharedEvent1();
        }

        public SharedEvent2 CreateSharedEvent2()
        {
            return new SharedEvent2();
        }
    }
}
