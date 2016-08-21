using Mendham.Events;
using System.Threading.Tasks;

namespace Mendham.Testing.Events.Test
{
    public class DomainEventPublisherFixtureTestingFixture : Fixture<DomainEventPublisherFixture>
    {
        private DomainEventPublisherFixture _sut;
        private IEventPublisher _domainEventPublisher;

        public override DomainEventPublisherFixture CreateSut()
        {
            return _sut;
        }

        public override void ResetFixture()
        {
            base.ResetFixture();

            _sut = new DomainEventPublisherFixture();
            _domainEventPublisher = _sut.GetDomainEventPublisher();
        }

        public Task RaiseTestEvent1()
        {
            return _domainEventPublisher.RaiseAsync(new TestEvent1());
        }

        public Task RaiseTestEvent2(string value)
        {
            return _domainEventPublisher.RaiseAsync(new TestEvent2(value));
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
