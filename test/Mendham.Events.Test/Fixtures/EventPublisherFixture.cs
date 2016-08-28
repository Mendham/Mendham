using Mendham.Events.Components;
using Mendham.Events.Test.TestObjects;
using Mendham.Testing;
using Moq;
using System.Collections.Generic;

namespace Mendham.Events.Test.Fixtures
{
    public class EventPublisherFixture : Fixture<EventPublisher>
    {
		public IEventHandlerContainer EventHandlerContainer { get; set; }
        public IEventHandlerProcessor EventHandlerProcessor { get; set; }
        public IEventLoggerProcessor EventLoggerProcessor { get; set; }

        public override EventPublisher CreateSut()
		{
            var components = new EventPublisherComponents(EventHandlerContainer,
                EventHandlerProcessor, EventLoggerProcessor);

            return new EventPublisher(components);
		}

		public override void ResetFixture()
		{
			base.ResetFixture();

			EventHandlerContainer = Mock.Of<IEventHandlerContainer>();
            EventHandlerProcessor = Mock.Of<IEventHandlerProcessor>();
            EventLoggerProcessor = Mock.Of<IEventLoggerProcessor>();
        }

        public TestEvent CreateEvent()
        {
            return new TestEvent();
        }

        public IEnumerable<IEventHandler<TestEvent>> GetEventHandlersForTestEvent()
        {
            return Mock.Of<IEnumerable<IEventHandler<TestEvent>>>();
        }
	}
}
