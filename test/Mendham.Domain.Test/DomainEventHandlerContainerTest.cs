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
		private readonly IDomainEventHandler<BaseDomainEvent> _baseEventHandler;
		private readonly IDomainEventHandler<DerivedDomainEvent> _derivedEventHandler;
		private readonly IDomainEventHandler<OtherDomainEvent> _otherEventHandler;

		public DomainEventHandlerContainerTest(DomainEventHandlerContainerFixture fixture) : base(fixture)
		{
			_baseEventHandler = Mock.Of<IDomainEventHandler<BaseDomainEvent>>();
			_derivedEventHandler = Mock.Of<IDomainEventHandler<DerivedDomainEvent>>();
			_otherEventHandler = Mock.Of<IDomainEventHandler<OtherDomainEvent>>();

			_fixture.DomainEventHandlers = new List<IDomainEventHandler>
			{
				_baseEventHandler,
				_derivedEventHandler,
				_otherEventHandler
			};
		}

		public class BaseDomainEvent : DomainEvent
		{ }

		public class DerivedDomainEvent : BaseDomainEvent
		{ }

		public class OtherDomainEvent : DomainEvent
		{ }

		[Fact]
		public async Task BaseHandler_HandledBy_BaseEvent()
		{
			var baseEvent = new BaseDomainEvent();

			_baseEventHandler.AsMock()
				.Setup(a => a.HandleAsync(baseEvent))
				.ReturnsNoActionTask();

			var sut = _fixture.CreateSut();

			await sut.HandleAllAsync(baseEvent);

			_baseEventHandler.AsMock()
				.Verify(a => a.HandleAsync(baseEvent), Times.Once);
		}

		[Fact]
		public async Task DerivedHandler_NotHandledBy_BaseEvent()
		{
			var baseEvent = new BaseDomainEvent();

			var sut = _fixture.CreateSut();

			await sut.HandleAllAsync(baseEvent);

			_derivedEventHandler.AsMock()
				.Verify(a => a.HandleAsync(It.IsAny<DerivedDomainEvent>()), Times.Never);
		}

		[Fact]
		public async Task OtherHandler_NotHandledBy_BaseEvent()
		{
			var baseEvent = new BaseDomainEvent();

			var sut = _fixture.CreateSut();

			await sut.HandleAllAsync(baseEvent);

			_otherEventHandler.AsMock()
				.Verify(a => a.HandleAsync(It.IsAny<OtherDomainEvent>()), Times.Never);
		}

		[Fact]
		public async Task BaseHandler_HandledBy_DerivedEvent()
		{
			var derivedEvent = new DerivedDomainEvent();

			_baseEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.ReturnsNoActionTask();

			var sut = _fixture.CreateSut();

			await sut.HandleAllAsync(derivedEvent);

			_baseEventHandler.AsMock()
				.Verify(a => a.HandleAsync(derivedEvent), Times.Once);
		}

		[Fact]
		public async Task DerivedHandler_HandledBy_DerivedEvent()
		{
			var derivedEvent = new DerivedDomainEvent();

			_derivedEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.ReturnsNoActionTask();

			var sut = _fixture.CreateSut();

			await sut.HandleAllAsync(derivedEvent);

			_derivedEventHandler.AsMock()
				.Verify(a => a.HandleAsync(derivedEvent), Times.Once);
		}

		[Fact]
		public async Task OtherHandler_NotHandledBy_DerivedEvent()
		{
			var derivedEvent = new DerivedDomainEvent();

			var sut = _fixture.CreateSut();

			await sut.HandleAllAsync(derivedEvent);

			_otherEventHandler.AsMock()
				.Verify(a => a.HandleAsync(It.IsAny<OtherDomainEvent>()), Times.Never);
		}

		[Fact]
		public void OneHandlerTaskThrowsException_Returns_SingleDomainEventHandlingException()
		{
			var derivedEvent = new DerivedDomainEvent();
			var exceptionFromHandler = new InvalidOperationException("Test exception");
			Func<Task> exTask = async () =>
			{
				await Task.FromResult(0);
				throw exceptionFromHandler;
			};

			var sut = _fixture.CreateSut();

			_derivedEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.Returns(exTask());

			Func<Task> act = () => sut.HandleAllAsync(derivedEvent);

			act.ShouldThrow<DomainEventHandlingException>()
				.Where(a => a.InnerException == exceptionFromHandler)
				.Where(a => a.DomainEvent == derivedEvent)
				.Which.DomainEventHandlerType.Should().Be(_derivedEventHandler.GetType());
		}

		[Fact]
		public void OneHandlerThrowsException_Returns_SingleDomainEventHandlingException()
		{
			var derivedEvent = new DerivedDomainEvent();
			var exceptionFromHandler = new InvalidOperationException("Test exception");
			Func<Task> exTask = () => { throw exceptionFromHandler; };

			var sut = _fixture.CreateSut();

			_derivedEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.Throws(exceptionFromHandler);

			Func<Task> act = () => sut.HandleAllAsync(derivedEvent);

			act.ShouldThrow<DomainEventHandlingException>()
				.Where(a => a.InnerException == exceptionFromHandler)
				.Where(a => a.DomainEvent == derivedEvent)
				.Which.DomainEventHandlerType.Should().Be(_derivedEventHandler.GetType());
		}

		[Fact]
		public void MultipleHandlerThrowExceptions_Returns_DomainEventHandlingException()
		{
			var derivedEvent = new DerivedDomainEvent();
			var exceptionFromHandler = new InvalidOperationException("Test exception");
			Func<Task> exTask = () => { throw exceptionFromHandler; };

			var sut = _fixture.CreateSut();

			_derivedEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.Throws(exceptionFromHandler);
			_baseEventHandler.AsMock()
				.Setup(a => a.HandleAsync(derivedEvent))
				.Throws(exceptionFromHandler);

			Func<Task> act = () => sut.HandleAllAsync(derivedEvent);

			act.ShouldThrow<DomainEventHandlingException>();
		}
	}
}
