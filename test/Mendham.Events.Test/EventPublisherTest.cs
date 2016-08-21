using Mendham.Events.Test.Fixtures;
using Mendham.Events.Test.TestObjects;
using Mendham.Testing;
using Mendham.Testing.Moq;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Events.Test
{
    public class EventPublisherTest : UnitTest<EventPublisherFixture>
	{
		public EventPublisherTest(EventPublisherFixture fixture) : base(fixture)
		{ }

		[Fact]
		public async Task RaiseAsync_Event_RaisesHandlers()
		{
			var testEvent = Fixture.CreateEvent();
            var handlers = Fixture.GetEventHandlersForTestEvent();

            Fixture.EventHandlerContainer.AsMock()
                .Setup(a => a.GetHandlers<TestEvent>())
                .Returns(handlers);
			Fixture.EventHandlerProcessor.AsMock()
				.Setup(a => a.HandleAllAsync(testEvent, handlers))
				.ReturnsNoActionTask();

			var sut = Fixture.CreateSut();

			await sut.RaiseAsync(testEvent);

			Fixture.EventHandlerProcessor.AsMock()
				.Verify(a => a.HandleAllAsync(testEvent, handlers), Times.Once);
		}

		[Fact]
		public async Task RaiseAsync_Event_LogsEvent()
		{
			var testEvent = Fixture.CreateEvent();
            var handlers = Fixture.GetEventHandlersForTestEvent();

            Fixture.EventHandlerContainer.AsMock()
                .Setup(a => a.GetHandlers<TestEvent>())
                .Returns(handlers);
            Fixture.EventHandlerProcessor.AsMock()
				.Setup(a => a.HandleAllAsync(testEvent, handlers))
				.ReturnsNoActionTask();

			var sut = Fixture.CreateSut();

			await sut.RaiseAsync(testEvent);

            Fixture.EventLoggerProcessor.AsMock()
                 .Verify(a => a.LogEvent(testEvent), Times.Once);
        }

		[Fact]
		public async Task RaiseAsync_HandlerException_StillLogsEvent()
		{
			var testEvent = Fixture.CreateEvent();
            var handlers = Fixture.GetEventHandlersForTestEvent();

            Fixture.EventHandlerContainer.AsMock()
                .Setup(a => a.GetHandlers<TestEvent>())
                .Returns(handlers);
            Fixture.EventHandlerProcessor.AsMock()
                .Setup(a => a.HandleAllAsync(testEvent, handlers))
                .Throws<InvalidOperationException>();

			var sut = Fixture.CreateSut();

			await Assert.ThrowsAsync<InvalidOperationException>(
				() => sut.RaiseAsync(testEvent));

            Fixture.EventLoggerProcessor.AsMock()
                .Verify(a => a.LogEvent(testEvent), Times.Once); ;
        }
	}
}
