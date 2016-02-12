using Mendham.Domain.Events;
using Mendham.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.Fixtures
{
    public class DomainEventLoggerContainerFixture : Fixture<DomainEventLoggerContainer>
    {
        public List<IDomainEventLogger> Loggers { get; set; }

        public override DomainEventLoggerContainer CreateSut()
        {
            return new DomainEventLoggerContainer(Loggers);
        }

        public override void ResetFixture()
        {
            base.ResetFixture();

            this.Loggers = new List<IDomainEventLogger>();
        }

        public class TestDomainEvent : DomainEvent
        { }

        public TestDomainEvent CreateDomainEvent()
        {
            return new TestDomainEvent();
        }
    }
}
