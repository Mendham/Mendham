using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Mendham.Domain.Events;
using Mendham.Domain.Test.Fixtures;
using Mendham.Testing;
using Moq;
using Xunit;

namespace Mendham.Domain.Test
{
	public class DomainEventHandlerContainerTest : BaseUnitTest<DomainEventHandlerContainerFixture>
	{
		public DomainEventHandlerContainerTest(DomainEventHandlerContainerFixture fixture) : base(fixture)
		{ }

		[Fact]
		public async Task BaseHandler_HandledBy_BaseEvent()
		{
			var baseEvent = _fixture.CreateBaseDomainEvent();

			_fixture.BaseEventHandler.AsMock()
				.Setup(a => a.HandleAsync(baseEvent))
				.ReturnsNoActionTask();

			var sut = _fixture.CreateSut();

			await sut.HandleAllAsync(baseEvent);

			_fixture.BaseEventHandler.AsMock()
				.Verify(a => a.HandleAsync(baseEvent), Times.Once);
		}

		[Fact]
		public async Task DerivedHandler_NotHandledBy_BaseEvent()
		{
			var baseEvent = _fixture.CreateBaseDomainEvent();

			var sut = _fixture.CreateSut();

			await sut.HandleAllAsync(baseEvent);

			_fixture.DerivedEventHandler.AsMock()
				.Verify(a => a.HandleAsync(It.IsAny<DomainEventHandlerContainerFixture.DerivedDomainEvent>()), Times.Never);
		}

		[Fact]
		public async Task OtherHandler_NotHandledBy_BaseEvent()
		{
			var baseEvent = _fixture.CreateBaseDomainEvent();

			var sut = _fixture.CreateSut();

			await sut.HandleAllAsync(baseEvent);

			_fixture.OtherEventHandler.AsMock()
				.Verify(a => a.HandleAsync(It.IsAny<DomainEventHandlerContainerFixture.OtherDomainEvent>()), Times.Never);
		}

		[Fact]
		public async Task BaseHandler_HandledBy_DerivedEvent()
		{
			var derivedEvent = _fixture.CreateDerivedDomainEvent();

			_fixture.BaseEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.ReturnsNoActionTask();

			var sut = _fixture.CreateSut();

			await sut.HandleAllAsync(derivedEvent);

			_fixture.BaseEventHandler.AsMock()
				.Verify(a => a.HandleAsync(derivedEvent), Times.Once);
		}

		[Fact]
		public async Task DerivedHandler_HandledBy_DerivedEvent()
		{
			var derivedEvent = _fixture.CreateDerivedDomainEvent();

			_fixture.DerivedEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.ReturnsNoActionTask();

			var sut = _fixture.CreateSut();

			await sut.HandleAllAsync(derivedEvent);

			_fixture.DerivedEventHandler.AsMock()
				.Verify(a => a.HandleAsync(derivedEvent), Times.Once);
		}

		[Fact]
		public async Task OtherHandler_NotHandledBy_DerivedEvent()
		{
			var derivedEvent = _fixture.CreateDerivedDomainEvent();

			var sut = _fixture.CreateSut();

			await sut.HandleAllAsync(derivedEvent);

			_fixture.OtherEventHandler.AsMock()
				.Verify(a => a.HandleAsync(It.IsAny<DomainEventHandlerContainerFixture.OtherDomainEvent>()), Times.Never);
		}

		[Fact]
		public void OneHandlerTaskThrowsException_Returns_SingleDomainEventHandlingException()
		{
			var derivedEvent = _fixture.CreateDerivedDomainEvent();
			var exceptionFromHandler = new InvalidOperationException("Test exception");
			Func<Task> exTask = async () =>
			{
				await Task.FromResult(0);
				throw exceptionFromHandler;
			};

			var sut = _fixture.CreateSut();

			_fixture.DerivedEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.Returns(exTask());

			Func<Task> act = () => sut.HandleAllAsync(derivedEvent);

			act.ShouldThrow<DomainEventHandlingException>()
				.Where(a => a.InnerException == exceptionFromHandler)
				.Where(a => a.DomainEvent == derivedEvent)
				.Which.DomainEventHandlerType.Should().Be(_fixture.DerivedEventHandler.GetType());
		}

		[Fact]
		public void OneHandlerThrowsException_Returns_SingleDomainEventHandlingException()
		{
			var derivedEvent = _fixture.CreateDerivedDomainEvent();
			var exceptionFromHandler = new InvalidOperationException("Test exception");
			Func<Task> exTask = () => { throw exceptionFromHandler; };

			var sut = _fixture.CreateSut();

			_fixture.DerivedEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.Throws(exceptionFromHandler);

			Func<Task> act = () => sut.HandleAllAsync(derivedEvent);

			act.ShouldThrow<DomainEventHandlingException>()
				.Where(a => a.InnerException == exceptionFromHandler)
				.Where(a => a.DomainEvent == derivedEvent)
				.Which.DomainEventHandlerType.Should().Be(_fixture.DerivedEventHandler.GetType());
		}

		[Fact]
		public void MultipleHandlerThrowExceptions_Returns_DomainEventHandlingException()
		{
			var derivedEvent = _fixture.CreateDerivedDomainEvent();
			var exceptionFromHandler = new InvalidOperationException("Test exception");
			Func<Task> exTask = () => { throw exceptionFromHandler; };

			var sut = _fixture.CreateSut();

			_fixture.DerivedEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.Throws(exceptionFromHandler);
			_fixture.BaseEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.Throws(exceptionFromHandler);

			Func<Task> act = () => sut.HandleAllAsync(derivedEvent);

			act.ShouldThrow<DomainEventHandlingException>();
		}
	}
}
