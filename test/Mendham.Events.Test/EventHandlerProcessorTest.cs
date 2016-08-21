using FluentAssertions;
using Mendham.Events.Test.Fixtures;
using Mendham.Events.Test.TestObjects;
using Mendham.Testing;
using Mendham.Testing.Moq;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Events.Test
{
    public class EventHandlerProcessorTest : UnitTest<EventHandlerProcessorFixture>
    {
        public EventHandlerProcessorTest(EventHandlerProcessorFixture fixture) : base(fixture)
        { }

        [Fact]
        public async Task HandleAllAsync_SingleHandlerForEvent_Handles()
        {
            Fixture.EventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.Event))
                .ReturnsNoActionTask();

            var sut = Fixture.CreateSut();

            await sut.HandleAllAsync(Fixture.Event, Fixture.GetFirstEventHandlerOnly());

            Fixture.EventHandler1.AsMock()
                .Verify(a => a.HandleAsync(Fixture.Event), Times.Once);
        }

        [Fact]
        public async Task HandleAllAsync_MultipleHandlerForEvent_HandlesAll()
        {
            Fixture.EventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.Event))
                .ReturnsNoActionTask();
            Fixture.EventHandler2.AsMock()
                .Setup(a => a.HandleAsync(Fixture.Event))
                .ReturnsNoActionTask();

            var sut = Fixture.CreateSut();

            await sut.HandleAllAsync(Fixture.Event, Fixture.GetAllEventHandlers());

            Fixture.EventHandler1.AsMock()
                .Verify(a => a.HandleAsync(Fixture.Event), Times.Once);
            Fixture.EventHandler2.AsMock()
                .Verify(a => a.HandleAsync(Fixture.Event), Times.Once);
        }

        [Fact]
        public async Task HandleAllAsync_NoHandlerForEvent_Completes()
        {
            var sut = Fixture.CreateSut();

            await sut.HandleAllAsync(Fixture.Event, Enumerable.Empty<IEventHandler<TestEvent>>());
        }

        [Fact]
        public async Task HandleAllAsync_TestEvent_LogEventHandlerStartBeforeHandling()
        {
            Fixture.EventHandlerLogger.AsMock()
                .Setup(a => a.LogEventHandlerStart(Fixture.EventHandler1.GetType(), Fixture.Event))
                .Callback(() => Fixture.EventHandler1.AsMock()
                    .Verify(a => a.HandleAsync(It.IsAny<TestEvent>()), Times.Never));

            var sut = Fixture.CreateSut();

            await sut.HandleAllAsync(Fixture.Event, Fixture.GetFirstEventHandlerOnly());

            Fixture.EventHandlerLogger.AsMock()
                .Verify(a => a.LogEventHandlerStart(Fixture.EventHandler1.GetType(), Fixture.Event), Times.Once);
        }

        [Fact]
        public async Task HandleAllAsync_TestEvent_LogEventHandlerCompleteAfterHandling()
        {
            Fixture.EventHandlerLogger.AsMock()
                .Setup(a => a.LogEventHandlerComplete(Fixture.EventHandler1.GetType(), Fixture.Event))
                .Callback(() => Fixture.EventHandler1.AsMock()
                    .Verify(a => a.HandleAsync(It.IsAny<TestEvent>()), Times.Once));

            var sut = Fixture.CreateSut();

            await sut.HandleAllAsync(Fixture.Event, Fixture.GetFirstEventHandlerOnly());

            Fixture.EventHandlerLogger.AsMock()
                .Verify(a => a.LogEventHandlerComplete(Fixture.EventHandler1.GetType(), Fixture.Event), Times.Once);
        }

        [Fact]
        public async Task HandleAllAsync_HandlerTaskThrowsException_ThrowsEventHandlingException()
        {
            var exceptionFromHandler = new InvalidOperationException("Test exception");
            Func<Task> exTask = async () =>
            {
                await Task.FromResult(0);
                throw exceptionFromHandler;
            };

            var sut = Fixture.CreateSut();

            Fixture.EventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.Event))
                .Returns(exTask());

            var ex = await Assert.ThrowsAsync<EventHandlingException>(
                () => sut.HandleAllAsync(Fixture.Event, Fixture.GetFirstEventHandlerOnly()));

            ex.InnerException.ShouldBeEquivalentTo(exceptionFromHandler);
            ex.Event.ShouldBeEquivalentTo(Fixture.Event);
            ex.EventHandlerType.ShouldBeEquivalentTo(Fixture.EventHandler1.GetType());
        }

        [Fact]
        public async Task HandleAllAsync_HandlerThrowsException_ThrowsEventHandlingException()
        {
            var exceptionFromHandler = new InvalidOperationException("Test exception");
            Func<Task> exTask = async () =>
            {
                await Task.FromResult(0);
                throw exceptionFromHandler;
            };

            var sut = Fixture.CreateSut();

            Fixture.EventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.Event))
                .Returns(exTask());

            var ex = await Assert.ThrowsAsync<EventHandlingException>(
                () => sut.HandleAllAsync(Fixture.Event, Fixture.GetFirstEventHandlerOnly()));

            ex.InnerException.ShouldBeEquivalentTo(exceptionFromHandler);
            ex.Event.ShouldBeEquivalentTo(Fixture.Event);
            ex.EventHandlerType.ShouldBeEquivalentTo(Fixture.EventHandler1.GetType());
        }

        [Fact]
        public async Task HandleAllAsync_HandlersThrowMultipleExceptions_ThrowsAggregateEventHandlingException()
        {
            Func<Task> exTask = async () =>
            {
                await Task.FromResult(0);
                throw new InvalidOperationException("Test exception");
            };

            var sut = Fixture.CreateSut();

            Fixture.EventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.Event))
                .Returns(exTask());
            Fixture.EventHandler2.AsMock()
                .Setup(a => a.HandleAsync(Fixture.Event))
                .Returns(exTask());

            var ex = await Assert.ThrowsAsync<AggregateEventHandlingException>(
                () => sut.HandleAllAsync(Fixture.Event, Fixture.GetAllEventHandlers()));

            ex.Event.ShouldBeEquivalentTo(Fixture.Event);
            ex.InnerException.Should().BeOfType<EventHandlingException>();
            ex.InnerExceptions.Should().HaveCount(2);
            ex.EventHandlerTypes.Should()
                .HaveCount(2)
                .And.Contain(Fixture.EventHandler1.GetType())
                .And.Contain(Fixture.EventHandler2.GetType());
        }

        [Fact]
        public async Task HandleAllAsync_HandlersThrowException_LogsEventHandlerError()
        {
            var ex = new InvalidOperationException("TestException");
            Func<Task> exTask = async () =>
            {
                await Task.FromResult(0);
                throw ex;
            };

            Fixture.EventHandlerLogger.AsMock()
                .Setup(a => a.LogEventHandlerComplete(Fixture.EventHandler1.GetType(), Fixture.Event))
                .Callback(() => Fixture.EventHandler1.AsMock()
                    .Verify(a => a.HandleAsync(It.IsAny<TestEvent>()), Times.Once));
            Fixture.EventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.Event))
                .Returns(exTask());

            var sut = Fixture.CreateSut();

            await Assert.ThrowsAnyAsync<Exception>(() => 
                sut.HandleAllAsync(Fixture.Event, Fixture.GetFirstEventHandlerOnly()));

            Fixture.EventHandlerLogger.AsMock()
                .Verify(a => a.LogEventHandlerError(Fixture.EventHandler1.GetType(), Fixture.Event, ex), Times.Once);
        }

        [Fact]
        public async Task HandleAllAsync_HandlersThrowMultipleExceptions_MultipleLogsEventHandlerError()
        {
            Func<Task> exTask = async () =>
            {
                await Task.FromResult(0);
                throw new InvalidOperationException("Test exception");
            };

            var sut = Fixture.CreateSut();

            Fixture.EventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.Event))
                .Returns(exTask());
            Fixture.EventHandler2.AsMock()
                .Setup(a => a.HandleAsync(Fixture.Event))
                .Returns(exTask());

            await Assert.ThrowsAsync<AggregateEventHandlingException>(
                () => sut.HandleAllAsync(Fixture.Event, Fixture.GetAllEventHandlers()));

            // This is a problem -------------------------
            Fixture.EventHandler1.GetType()
                .Should().NotBe(Fixture.EventHandler2.GetType(), "they are not the same");

            Fixture.EventHandlerLogger.AsMock()
                .Verify(a => a.LogEventHandlerError(
                    Fixture.EventHandler1.GetType(), Fixture.Event, It.IsAny<InvalidOperationException>()), Times.Once);
            Fixture.EventHandlerLogger.AsMock()
                .Verify(a => a.LogEventHandlerError(
                    Fixture.EventHandler2.GetType(), Fixture.Event, It.IsAny<InvalidOperationException>()), Times.Once);
        }

        [Fact]
        public async Task HandleAllAsync_HandlersThrowException_DoesNotFireLogsEventHandlerComplete()
        {
            Func<Task> exTask = async () =>
            {
                await Task.FromResult(0);
                throw new InvalidOperationException("Test exception");
            };

            Fixture.EventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.Event))
                .Returns(exTask());

            var sut = Fixture.CreateSut();

            await Assert.ThrowsAnyAsync<Exception>(() => 
                sut.HandleAllAsync(Fixture.Event, Fixture.GetFirstEventHandlerOnly()));

            Fixture.EventHandlerLogger.AsMock()
                .Verify(a => a.LogEventHandlerComplete(It.IsAny<Type>(), Fixture.Event), Times.Never);
        }
    }
}
