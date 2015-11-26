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
	public class DomainEventHandlerContainerTest : BaseUnitTest<DomainEventHandlerContainerFixture>
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
		public async Task HandleAllAsync_HandlerTaskThrowsExcpetion_ThrowsDomainEventHandlingException()
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
		public async Task HandleAllAsync_HandlerThrowsExcpetion_ThrowsDomainEventHandlingException()
		{
			var derivedEvent = Fixture.CreateDerivedDomainEvent();
			var exceptionFromHandler = new InvalidOperationException("Test exception");
			Func<Task> exTask = () => { throw exceptionFromHandler; };

			var sut = Fixture.CreateSut();

			Fixture.DerivedEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.Throws(exceptionFromHandler);

			var ex = await Assert.ThrowsAsync<DomainEventHandlingException>(
				() => sut.HandleAllAsync(derivedEvent));

			ex.InnerException.ShouldBeEquivalentTo(exceptionFromHandler);
			ex.DomainEvent.ShouldBeEquivalentTo(derivedEvent);
			ex.DomainEventHandlerType.ShouldBeEquivalentTo(Fixture.DerivedEventHandler.GetType());
		}

		[Fact]
		public async Task HandleAllAsync_HandlersThrowMultipleExcpetions_ThrowsDomainEventHandlingException()
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
				.Throws(exceptionFromHandler);
			Fixture.BaseEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.Throws(exceptionFromHandler);

			var ex = await Assert.ThrowsAsync<AggregateDomainEventHandlingException>(
				() => sut.HandleAllAsync(derivedEvent));

			ex.DomainEvent.ShouldBeEquivalentTo(derivedEvent);
			ex.InnerException.Should().BeOfType<DomainEventHandlingException>();
			ex.InnerExceptions.Should().HaveCount(2);
			ex.DomainEventHandlerTypes.Should().HaveCount(2);
			ex.DomainEventHandlerTypes.Should()
				.Contain(Fixture.DerivedEventHandler.GetType());
			ex.DomainEventHandlerTypes.Should()
				.Contain(Fixture.BaseEventHandler.GetType());
		}
	}
}