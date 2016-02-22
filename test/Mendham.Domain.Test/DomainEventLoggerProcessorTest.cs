using Mendham.Domain.Test.Fixtures;
using Mendham.Testing;
using Mendham.Testing.Moq;
using Moq;
using Xunit;

namespace Mendham.Domain.Test
{
    public class DomainEventLoggerProcessorTest : UnitTest<DomainEventLoggerProcessorFixture>
    {
        public DomainEventLoggerProcessorTest(DomainEventLoggerProcessorFixture fixture) : base(fixture)
        { }

        [Fact]
        public void LogDomainEventRaised_MultipleLoggers_AllLogged()
        {
            Fixture.AddLogger(Fixture.DomainEventLogger1);
            Fixture.AddLogger(Fixture.DomainEventLogger2);

            var sut = Fixture.CreateSut();

            sut.LogDomainEvent(Fixture.DomainEvent);

            Fixture.DomainEventLogger1.AsMock()
                .Verify(a => a.LogDomainEventRaised(Fixture.DomainEvent), Times.Once());
            Fixture.DomainEventLogger2.AsMock()
                .Verify(a => a.LogDomainEventRaised(Fixture.DomainEvent), Times.Once());
        }

        [Fact]
        public void LogDomainEventRaised_NoLoggers_Completes()
        {
            var sut = Fixture.CreateSut();

            sut.LogDomainEvent(Fixture.DomainEvent);
        }
    }
}
