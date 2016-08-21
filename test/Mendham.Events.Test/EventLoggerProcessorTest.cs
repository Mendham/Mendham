using Mendham.Events.Test.Fixtures;
using Mendham.Testing;
using Mendham.Testing.Moq;
using Moq;
using Xunit;

namespace Mendham.Events.Test
{
    public class EventLoggerProcessorTest : UnitTest<EventLoggerProcessorFixture>
    {
        public EventLoggerProcessorTest(EventLoggerProcessorFixture fixture) : base(fixture)
        { }

        [Fact]
        public void LogEventRaised_MultipleLoggers_AllLogged()
        {
            Fixture.AddLogger(Fixture.EventLogger1);
            Fixture.AddLogger(Fixture.EventLogger2);

            var sut = Fixture.CreateSut();

            sut.LogEvent(Fixture.Event);

            Fixture.EventLogger1.AsMock()
                .Verify(a => a.LogEvent(Fixture.Event), Times.Once());
            Fixture.EventLogger2.AsMock()
                .Verify(a => a.LogEvent(Fixture.Event), Times.Once());
        }

        [Fact]
        public void LogEventRaised_NoLoggers_Completes()
        {
            var sut = Fixture.CreateSut();

            sut.LogEvent(Fixture.Event);
        }
    }
}
