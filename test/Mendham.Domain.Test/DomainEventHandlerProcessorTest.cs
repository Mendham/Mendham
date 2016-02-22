using FluentAssertions;
using Mendham.Domain.Events;
using Mendham.Domain.Test.Fixtures;
using Mendham.Domain.Test.TestObjects.Events;
using Mendham.Testing;
using Mendham.Testing.Moq;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.Test
{
    public class DomainEventHandlerProcessorTest : UnitTest<DomainEventHandlerProcessorFixture>
    {
        public DomainEventHandlerProcessorTest(DomainEventHandlerProcessorFixture fixture) : base(fixture)
        { }

        [Fact]
        public async Task HandleAllAsync_SingleHandlerForEvent_Handles()
        {
            Fixture.DomainEventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.DomainEvent))
                .ReturnsNoActionTask();

            var sut = Fixture.CreateSut();

            await sut.HandleAllAsync(Fixture.DomainEvent, Fixture.GetFirstDomainEventHandlerOnly());

            Fixture.DomainEventHandler1.AsMock()
                .Verify(a => a.HandleAsync(Fixture.DomainEvent), Times.Once);
        }

        [Fact]
        public async Task HandleAllAsync_MultipleHandlerForEvent_HandlesAll()
        {
            Fixture.DomainEventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.DomainEvent))
                .ReturnsNoActionTask();
            Fixture.DomainEventHandler2.AsMock()
                .Setup(a => a.HandleAsync(Fixture.DomainEvent))
                .ReturnsNoActionTask();

            var sut = Fixture.CreateSut();

            await sut.HandleAllAsync(Fixture.DomainEvent, Fixture.GetAllDomainEventHandlers());

            Fixture.DomainEventHandler1.AsMock()
                .Verify(a => a.HandleAsync(Fixture.DomainEvent), Times.Once);
            Fixture.DomainEventHandler2.AsMock()
                .Verify(a => a.HandleAsync(Fixture.DomainEvent), Times.Once);
        }

        [Fact]
        public async Task HandleAllAsync_NoHandlerForEvent_Completes()
        {
            var sut = Fixture.CreateSut();

            await sut.HandleAllAsync(Fixture.DomainEvent, Enumerable.Empty<IDomainEventHandler<TestDomainEvent>>());
        }

        [Fact]
        public async Task HandleAllAsync_TestDomainEvent_LogDomainEventHandlerStartBeforeHandling()
        {
            Fixture.DomainEventHandlerLogger.AsMock()
                .Setup(a => a.LogDomainEventHandlerStart(Fixture.DomainEventHandler1.GetType(), Fixture.DomainEvent))
                .Callback(() => Fixture.DomainEventHandler1.AsMock()
                    .Verify(a => a.HandleAsync(It.IsAny<TestDomainEvent>()), Times.Never));

            var sut = Fixture.CreateSut();

            await sut.HandleAllAsync(Fixture.DomainEvent, Fixture.GetFirstDomainEventHandlerOnly());

            Fixture.DomainEventHandlerLogger.AsMock()
                .Verify(a => a.LogDomainEventHandlerStart(Fixture.DomainEventHandler1.GetType(), Fixture.DomainEvent), Times.Once);
        }

        [Fact]
        public async Task HandleAllAsync_TestDomainEvent_LogDomainEventHandlerCompleteAfterHandling()
        {
            Fixture.DomainEventHandlerLogger.AsMock()
                .Setup(a => a.LogDomainEventHandlerComplete(Fixture.DomainEventHandler1.GetType(), Fixture.DomainEvent))
                .Callback(() => Fixture.DomainEventHandler1.AsMock()
                    .Verify(a => a.HandleAsync(It.IsAny<TestDomainEvent>()), Times.Once));

            var sut = Fixture.CreateSut();

            await sut.HandleAllAsync(Fixture.DomainEvent, Fixture.GetFirstDomainEventHandlerOnly());

            Fixture.DomainEventHandlerLogger.AsMock()
                .Verify(a => a.LogDomainEventHandlerComplete(Fixture.DomainEventHandler1.GetType(), Fixture.DomainEvent), Times.Once);
        }

        [Fact]
        public async Task HandleAllAsync_HandlerTaskThrowsException_ThrowsDomainEventHandlingException()
        {
            var exceptionFromHandler = new InvalidOperationException("Test exception");
            Func<Task> exTask = async () =>
            {
                await Task.FromResult(0);
                throw exceptionFromHandler;
            };

            var sut = Fixture.CreateSut();

            Fixture.DomainEventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.DomainEvent))
                .Returns(exTask());

            var ex = await Assert.ThrowsAsync<DomainEventHandlingException>(
                () => sut.HandleAllAsync(Fixture.DomainEvent, Fixture.GetFirstDomainEventHandlerOnly()));

            ex.InnerException.ShouldBeEquivalentTo(exceptionFromHandler);
            ex.DomainEvent.ShouldBeEquivalentTo(Fixture.DomainEvent);
            ex.DomainEventHandlerType.ShouldBeEquivalentTo(Fixture.DomainEventHandler1.GetType());
        }

        [Fact]
        public async Task HandleAllAsync_HandlerThrowsException_ThrowsDomainEventHandlingException()
        {
            var exceptionFromHandler = new InvalidOperationException("Test exception");
            Func<Task> exTask = async () =>
            {
                await Task.FromResult(0);
                throw exceptionFromHandler;
            };

            var sut = Fixture.CreateSut();

            Fixture.DomainEventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.DomainEvent))
                .Returns(exTask());

            var ex = await Assert.ThrowsAsync<DomainEventHandlingException>(
                () => sut.HandleAllAsync(Fixture.DomainEvent, Fixture.GetFirstDomainEventHandlerOnly()));

            ex.InnerException.ShouldBeEquivalentTo(exceptionFromHandler);
            ex.DomainEvent.ShouldBeEquivalentTo(Fixture.DomainEvent);
            ex.DomainEventHandlerType.ShouldBeEquivalentTo(Fixture.DomainEventHandler1.GetType());
        }

        [Fact]
        public async Task HandleAllAsync_HandlersThrowMultipleExceptions_ThrowsAggregateDomainEventHandlingException()
        {
            Func<Task> exTask = async () =>
            {
                await Task.FromResult(0);
                throw new InvalidOperationException("Test exception");
            };

            var sut = Fixture.CreateSut();

            Fixture.DomainEventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.DomainEvent))
                .Returns(exTask());
            Fixture.DomainEventHandler2.AsMock()
                .Setup(a => a.HandleAsync(Fixture.DomainEvent))
                .Returns(exTask());

            var ex = await Assert.ThrowsAsync<AggregateDomainEventHandlingException>(
                () => sut.HandleAllAsync(Fixture.DomainEvent, Fixture.GetAllDomainEventHandlers()));

            ex.DomainEvent.ShouldBeEquivalentTo(Fixture.DomainEvent);
            ex.InnerException.Should().BeOfType<DomainEventHandlingException>();
            ex.InnerExceptions.Should().HaveCount(2);
            ex.DomainEventHandlerTypes.Should()
                .HaveCount(2)
                .And.Contain(Fixture.DomainEventHandler1.GetType())
                .And.Contain(Fixture.DomainEventHandler2.GetType());
        }

        [Fact]
        public async Task HandleAllAsync_HandlersThrowException_LogsDomainEventHandlerError()
        {
            var ex = new InvalidOperationException("TestException");
            Func<Task> exTask = async () =>
            {
                await Task.FromResult(0);
                throw ex;
            };

            Fixture.DomainEventHandlerLogger.AsMock()
                .Setup(a => a.LogDomainEventHandlerComplete(Fixture.DomainEventHandler1.GetType(), Fixture.DomainEvent))
                .Callback(() => Fixture.DomainEventHandler1.AsMock()
                    .Verify(a => a.HandleAsync(It.IsAny<TestDomainEvent>()), Times.Once));
            Fixture.DomainEventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.DomainEvent))
                .Returns(exTask());

            var sut = Fixture.CreateSut();

            await Assert.ThrowsAnyAsync<Exception>(() => 
                sut.HandleAllAsync(Fixture.DomainEvent, Fixture.GetFirstDomainEventHandlerOnly()));

            Fixture.DomainEventHandlerLogger.AsMock()
                .Verify(a => a.LogDomainEventHandlerError(Fixture.DomainEventHandler1.GetType(), Fixture.DomainEvent, ex), Times.Once);
        }

        [Fact]
        public async Task HandleAllAsync_HandlersThrowMultipleExceptions_MultipleLogsDomainEventHandlerError()
        {
            Func<Task> exTask = async () =>
            {
                await Task.FromResult(0);
                throw new InvalidOperationException("Test exception");
            };

            var sut = Fixture.CreateSut();

            Fixture.DomainEventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.DomainEvent))
                .Returns(exTask());
            Fixture.DomainEventHandler2.AsMock()
                .Setup(a => a.HandleAsync(Fixture.DomainEvent))
                .Returns(exTask());

            await Assert.ThrowsAsync<AggregateDomainEventHandlingException>(
                () => sut.HandleAllAsync(Fixture.DomainEvent, Fixture.GetAllDomainEventHandlers()));

            // This is a problem -------------------------
            Fixture.DomainEventHandler1.GetType()
                .Should().NotBe(Fixture.DomainEventHandler2.GetType(), "they are not the same");

            Fixture.DomainEventHandlerLogger.AsMock()
                .Verify(a => a.LogDomainEventHandlerError(
                    Fixture.DomainEventHandler1.GetType(), Fixture.DomainEvent, It.IsAny<InvalidOperationException>()), Times.Once);
            Fixture.DomainEventHandlerLogger.AsMock()
                .Verify(a => a.LogDomainEventHandlerError(
                    Fixture.DomainEventHandler2.GetType(), Fixture.DomainEvent, It.IsAny<InvalidOperationException>()), Times.Once);
        }

        [Fact]
        public async Task HandleAllAsync_HandlersThrowException_DoesNotFireLogsDomainEventHandlerComplete()
        {
            Func<Task> exTask = async () =>
            {
                await Task.FromResult(0);
                throw new InvalidOperationException("Test exception");
            };

            Fixture.DomainEventHandler1.AsMock()
                .Setup(a => a.HandleAsync(Fixture.DomainEvent))
                .Returns(exTask());

            var sut = Fixture.CreateSut();

            await Assert.ThrowsAnyAsync<Exception>(() => 
                sut.HandleAllAsync(Fixture.DomainEvent, Fixture.GetFirstDomainEventHandlerOnly()));

            Fixture.DomainEventHandlerLogger.AsMock()
                .Verify(a => a.LogDomainEventHandlerComplete(It.IsAny<Type>(), Fixture.DomainEvent), Times.Never);
        }
    }
}
