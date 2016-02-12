using Mendham.Domain.Events;
using Mendham.Domain.Test.Fixtures;
using Mendham.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.Test
{
    public class DomainEventLoggerContainerTest : UnitTest<DomainEventLoggerContainerFixture>
    {
        public DomainEventLoggerContainerTest(DomainEventLoggerContainerFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void WriteToAllLoggers_OneEvent_CallsAllLoggers()
        {
            var domainEvent = Fixture.CreateDomainEvent();
            var logger1Mock = new Mock<IDomainEventLogger>();
            var logger2Mock = new Mock<IDomainEventLogger>();

            Fixture.Loggers.Add(logger1Mock.Object);
            Fixture.Loggers.Add(logger2Mock.Object);

            var sut = Fixture.CreateSut();

            sut.WriteToAllLoggers(domainEvent);

            logger1Mock.Verify(a => a.LogDomainEvent(domainEvent), Times.Once);
            logger2Mock.Verify(a => a.LogDomainEvent(domainEvent), Times.Once);
        }
    }
}
