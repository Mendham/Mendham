using Mendham.Domain.Test.Fixtures;
using Mendham.Domain.Test.TestObjects.Events;
using Mendham.Testing;
using Mendham.Testing.Moq;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.Test
{
    public class DomainEventPublisherTest : UnitTest<DomainEventPublisherFixture>
	{
		public DomainEventPublisherTest(DomainEventPublisherFixture fixture) : base(fixture)
		{ }

		[Fact]
		public async Task RaiseAsync_DomainEvent_RaisesHandlers()
		{
			var domainEvent = Fixture.CreateDomainEvent();
            var handlers = Fixture.GetDomainEventHandlersForTestDomainEvent();

            Fixture.DomainEventHandlerContainer.AsMock()
                .Setup(a => a.GetHandlers<TestDomainEvent>())
                .Returns(handlers);
			Fixture.DomainEventHandlerProcessor.AsMock()
				.Setup(a => a.HandleAllAsync(domainEvent, handlers))
				.ReturnsNoActionTask();

			var sut = Fixture.CreateSut();

			await sut.RaiseAsync(domainEvent);

			Fixture.DomainEventHandlerProcessor.AsMock()
				.Verify(a => a.HandleAllAsync(domainEvent, handlers), Times.Once);
		}

		[Fact]
		public async Task RaiseAsync_DomainEvent_LogsEvent()
		{
			var domainEvent = Fixture.CreateDomainEvent();
            var handlers = Fixture.GetDomainEventHandlersForTestDomainEvent();

            Fixture.DomainEventHandlerContainer.AsMock()
                .Setup(a => a.GetHandlers<TestDomainEvent>())
                .Returns(handlers);
            Fixture.DomainEventHandlerProcessor.AsMock()
				.Setup(a => a.HandleAllAsync(domainEvent, handlers))
				.ReturnsNoActionTask();

			var sut = Fixture.CreateSut();

			await sut.RaiseAsync(domainEvent);

            Fixture.DomainEventLoggerProcessor.AsMock()
                 .Verify(a => a.LogDomainEvent(domainEvent), Times.Once);
        }

		[Fact]
		public async Task RaiseAsync_HandlerException_StillLogsEvent()
		{
			var domainEvent = Fixture.CreateDomainEvent();
            var handlers = Fixture.GetDomainEventHandlersForTestDomainEvent();

            Fixture.DomainEventHandlerContainer.AsMock()
                .Setup(a => a.GetHandlers<TestDomainEvent>())
                .Returns(handlers);
            Fixture.DomainEventHandlerProcessor.AsMock()
                .Setup(a => a.HandleAllAsync(domainEvent, handlers))
                .Throws<InvalidOperationException>();

			var sut = Fixture.CreateSut();

			await Assert.ThrowsAsync<InvalidOperationException>(
				() => sut.RaiseAsync(domainEvent));

            Fixture.DomainEventLoggerProcessor.AsMock()
                .Verify(a => a.LogDomainEvent(domainEvent), Times.Once); ;
        }
	}
}
