using FluentAssertions;
using Mendham.Domain.Events;
using Mendham.Domain.Test.Fixtures;
using Mendham.Testing;
using Mendham.Testing.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.Test
{
	public class DomainEventHandlerContainerTest : UnitTest<DomainEventHandlerContainerFixture>
	{
		public DomainEventHandlerContainerTest(DomainEventHandlerContainerFixture fixture) : base(fixture)
		{ }

		[Fact]
		public async Task HandleAllAsync_BaseEvent_HandlesBaseHandler()
		{
			var baseEvent = Fixture.CreateBaseDomainEvent();

			Fixture.BaseEventHandler.AsMock()
				.Setup(a => a.HandleAsync(baseEvent))
				.ReturnsNoActionTask();

			var sut = Fixture.CreateSut();

			await sut.HandleAllAsync(baseEvent);

			Fixture.BaseEventHandler.AsMock()
				.Verify(a => a.HandleAsync(baseEvent), Times.Once);
		}

		[Fact]
		public async Task HandleAllAsync_BaseEvent_DoesntHandlesDerivedHandler()
		{
			var baseEvent = Fixture.CreateBaseDomainEvent();

			var sut = Fixture.CreateSut();

			await sut.HandleAllAsync(baseEvent);

			Fixture.DerivedEventHandler.AsMock()
				.Verify(a => a.HandleAsync(It.IsAny<DomainEventHandlerContainerFixture.DerivedDomainEvent>()), Times.Never);
		}

		[Fact]
		public async Task HandleAllAsync_BaseEvent_DoesntHandlesOtherHandler()
		{
			var baseEvent = Fixture.CreateBaseDomainEvent();

			var sut = Fixture.CreateSut();

			await sut.HandleAllAsync(baseEvent);

			Fixture.OtherEventHandler.AsMock()
				.Verify(a => a.HandleAsync(It.IsAny<DomainEventHandlerContainerFixture.OtherDomainEvent>()), Times.Never);
		}

        [Fact]
        public async Task HandleAllAsync_BaseEvent_LogDomainEventHandlerStartBeforeHandling()
        {
            var domainEvent = Fixture.CreateBaseDomainEvent();
            Fixture.DomainEventHandlerLogger.AsMock()
                .Setup(a => a.LogDomainEventHandlerStart(Fixture.BaseEventHandler.GetType(), domainEvent))
                .Callback(() => Fixture.BaseEventHandler.AsMock()
                    .Verify(a => a.HandleAsync(It.IsAny<DomainEventHandlerContainerFixture.BaseDomainEvent>()), Times.Never));

            var sut = Fixture.CreateSut();

            await sut.HandleAllAsync(domainEvent);

            Fixture.DomainEventHandlerLogger.AsMock()
                .Verify(a => a.LogDomainEventHandlerStart(Fixture.BaseEventHandler.GetType(), domainEvent), Times.Once);
        }

        [Fact]
        public async Task HandleAllAsync_BaseEvent_LogDomainEventHandlerCompleteAfterHandling()
        {
            var domainEvent = Fixture.CreateBaseDomainEvent();
            Fixture.DomainEventHandlerLogger.AsMock()
                .Setup(a => a.LogDomainEventHandlerComplete(Fixture.BaseEventHandler.GetType(), domainEvent))
                .Callback(() => Fixture.BaseEventHandler.AsMock()
                    .Verify(a => a.HandleAsync(It.IsAny<DomainEventHandlerContainerFixture.BaseDomainEvent>()), Times.Once));

            var sut = Fixture.CreateSut();

            await sut.HandleAllAsync(domainEvent);

            Fixture.DomainEventHandlerLogger.AsMock()
                .Verify(a => a.LogDomainEventHandlerComplete(Fixture.BaseEventHandler.GetType(), domainEvent), Times.Once);
        }

        [Fact]
		public async Task HandleAllAsync_DerivedEvent_HandlesBaseHandler()
		{
			var derivedEvent = Fixture.CreateDerivedDomainEvent();

			Fixture.BaseEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.ReturnsNoActionTask();

			var sut = Fixture.CreateSut();

			await sut.HandleAllAsync(derivedEvent);

			Fixture.BaseEventHandler.AsMock()
				.Verify(a => a.HandleAsync(derivedEvent), Times.Once);
		}

		[Fact]
		public async Task HandleAllAsync_DerivedEvent_HandlesDerivedHandler()
		{
			var derivedEvent = Fixture.CreateDerivedDomainEvent();

			Fixture.DerivedEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.ReturnsNoActionTask();

			var sut = Fixture.CreateSut();

			await sut.HandleAllAsync(derivedEvent);

			Fixture.DerivedEventHandler.AsMock()
				.Verify(a => a.HandleAsync(derivedEvent), Times.Once);
		}

		[Fact]
		public async Task HandleAllAsync_DerivedEvent_DoesntHandlesOtherHandler()
		{
			var derivedEvent = Fixture.CreateDerivedDomainEvent();

			var sut = Fixture.CreateSut();

			await sut.HandleAllAsync(derivedEvent);

			Fixture.OtherEventHandler.AsMock()
				.Verify(a => a.HandleAsync(It.IsAny<DomainEventHandlerContainerFixture.OtherDomainEvent>()), Times.Never);
		}

		[Fact]
		public async Task HandleAllAsync_HandlerTaskThrowsException_ThrowsDomainEventHandlingException()
		{
			var derivedEvent = Fixture.CreateDerivedDomainEvent();
			var exceptionFromHandler = new InvalidOperationException("Test exception");
			Func<Task> exTask = async () =>
			{
				await Task.FromResult(0);
				throw exceptionFromHandler;
			};

			var sut = Fixture.CreateSut();

			Fixture.DerivedEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.Returns(exTask());

			var ex = await Assert.ThrowsAsync<DomainEventHandlingException>(
				() => sut.HandleAllAsync(derivedEvent));

			ex.InnerException.ShouldBeEquivalentTo(exceptionFromHandler);
			ex.DomainEvent.ShouldBeEquivalentTo(derivedEvent);
			ex.DomainEventHandlerType.ShouldBeEquivalentTo(Fixture.DerivedEventHandler.GetType());
		}

		[Fact]
		public async Task HandleAllAsync_HandlerThrowsException_ThrowsDomainEventHandlingException()
		{
			var derivedEvent = Fixture.CreateDerivedDomainEvent();
			var exceptionFromHandler = new InvalidOperationException("Test exception");
			Func<Task> exTask = async () =>
            {
                await Task.FromResult(0);
                throw exceptionFromHandler;
            };

            var sut = Fixture.CreateSut();

            Fixture.DerivedEventHandler.AsMock()
                .Setup(a => a.HandleAsync(derivedEvent))
                .Returns(exTask());

			var ex = await Assert.ThrowsAsync<DomainEventHandlingException>(
				() => sut.HandleAllAsync(derivedEvent));

			ex.InnerException.ShouldBeEquivalentTo(exceptionFromHandler);
			ex.DomainEvent.ShouldBeEquivalentTo(derivedEvent);
			ex.DomainEventHandlerType.ShouldBeEquivalentTo(Fixture.DerivedEventHandler.GetType());
		}

		[Fact]
		public async Task HandleAllAsync_HandlersThrowMultipleExceptions_ThrowsDomainEventHandlingException()
		{
			var derivedEvent = Fixture.CreateDerivedDomainEvent();
			Func<Task> exTask = async () =>
			{
				await Task.FromResult(0);
				throw new InvalidOperationException("Test exception");
            };

			var sut = Fixture.CreateSut();

			Fixture.DerivedEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
                .Returns(exTask());
            Fixture.BaseEventHandler.AsMock()
                .Setup(a => a.HandleAsync(derivedEvent))
                .Returns(exTask());

			var ex = await Assert.ThrowsAsync<AggregateDomainEventHandlingException>(
				() => sut.HandleAllAsync(derivedEvent));

			ex.DomainEvent.ShouldBeEquivalentTo(derivedEvent);
			ex.InnerException.Should().BeOfType<DomainEventHandlingException>();
			ex.InnerExceptions.Should().HaveCount(2);
			ex.DomainEventHandlerTypes.Should()
                .HaveCount(2)
                .And.Contain(Fixture.DerivedEventHandler.GetType())
                .And.Contain(Fixture.BaseEventHandler.GetType());
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

            var domainEvent = Fixture.CreateBaseDomainEvent();
            Fixture.DomainEventHandlerLogger.AsMock()
                .Setup(a => a.LogDomainEventHandlerComplete(Fixture.BaseEventHandler.GetType(), domainEvent))
                .Callback(() => Fixture.BaseEventHandler.AsMock()
                    .Verify(a => a.HandleAsync(It.IsAny<DomainEventHandlerContainerFixture.BaseDomainEvent>()), Times.Once));
            Fixture.BaseEventHandler.AsMock()
                .Setup(a => a.HandleAsync(domainEvent))
                .Returns(exTask());

            var sut = Fixture.CreateSut();

            await Assert.ThrowsAnyAsync<Exception>(() => sut.HandleAllAsync(domainEvent));

            Fixture.DomainEventHandlerLogger.AsMock()
                .Verify(a => a.LogDomainEventHandlerError(Fixture.BaseEventHandler.GetType(), domainEvent, ex), Times.Once);
        }
    }
}