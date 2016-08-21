using Mendham.Events;
using System.Threading.Tasks;

namespace Mendham.Testing.Events.Test
{
    public class EventPublisherFixtureTestingFixture : Fixture<EventPublisherFixture>
    {
        private EventPublisherFixture _sut;
        private IEventPublisher _eventPublisher;

        public override EventPublisherFixture CreateSut()
        {
            return _sut;
        }

        public override void ResetFixture()
        {
            base.ResetFixture();

            _sut = new EventPublisherFixture();
            _eventPublisher = _sut.GetEventPublisher();
        }

        public Task RaiseTestEvent1()
        {
            return _eventPublisher.RaiseAsync(new TestEvent1());
        }

        public Task RaiseTestEvent2(string value)
        {
            return _eventPublisher.RaiseAsync(new TestEvent2(value));
        }

        public class TestEvent1 : Event
        { }

        public class TestEvent2 : Event
        {
            public string Value { get; private set; }

            public TestEvent2(string value)
            {
                Value = value;
            }
        }
    }
}
