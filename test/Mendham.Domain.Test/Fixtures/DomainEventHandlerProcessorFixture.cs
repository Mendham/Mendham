using Mendham.Events.Components;
using Mendham.Events.Test.TestObjects;
using Mendham.Testing;
using Moq;
using System.Collections.Generic;

namespace Mendham.Events.Test.Fixtures
{
    public class EventHandlerProcessorFixture : Fixture<EventHandlerProcessor>
    {
        public IEventHandler<TestEvent> EventHandler1 { get; set; }
        public IEventHandler<TestEvent> EventHandler2 { get; set; }

        public IEventHandlerLogger EventHandlerLogger { get; set; }

        public TestEvent Event { get; set; }

        public override EventHandlerProcessor CreateSut()
        {
            return new EventHandlerProcessor(EventHandlerLogger.AsSingleItemEnumerable());
        }

        public override void ResetFixture()
        {
            base.ResetFixture();

            EventHandlerLogger = Mock.Of<IEventHandlerLogger>();

            EventHandler1 = Mock.Of<IEventHandler<TestEvent>>();
            EventHandler2 = Mock.Of<AltTestEventHandlerInterface>();

            Event = new TestEvent();
        }

        public IEnumerable<IEventHandler<TestEvent>> GetAllEventHandlers()
        {
            yield return EventHandler1;
            yield return EventHandler2;
        }

        public IEnumerable<IEventHandler<TestEvent>> GetFirstEventHandlerOnly()
        {
            yield return EventHandler1;
        }

        // This is just done as a trick to make mock think the second interface is a different type
        public interface AltTestEventHandlerInterface : IEventHandler<TestEvent>
        { }
    }
}