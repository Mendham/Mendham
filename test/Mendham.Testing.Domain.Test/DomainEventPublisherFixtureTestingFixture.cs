using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Domain.Test
{
    public class DomainEventPublisherFixtureTestingFixture : Fixture<DomainEventPublisherFixture>
    {
        private DomainEventPublisherFixture sut;
        private IDomainEventPublisher domainEventPublisher;

        public override DomainEventPublisherFixture CreateSut()
        {
            return sut;
        }

        public override void ResetFixture()
        {
            base.ResetFixture();

            sut = new DomainEventPublisherFixture();
            domainEventPublisher = sut.GetDomainEventPublisherProvider().GetPublisher();
        }

        public Task RaiseTestDomainEvent1()
        {
            return domainEventPublisher.RaiseAsync(new TestDomainEvent1());
        }

        public Task RaiseTestDomainEvent2(string value)
        {
            return domainEventPublisher.RaiseAsync(new TestDomainEvent2(value));
        }

        public class TestDomainEvent1 : DomainEvent
        { }

        public class TestDomainEvent2 : DomainEvent
        {
            public string Value { get; private set; }

            public TestDomainEvent2(string value)
            {
                this.Value = value;
            }
        }
    }
}
