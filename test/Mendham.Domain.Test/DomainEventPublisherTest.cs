using Mendham.Domain.Events;
using Mendham.Domain.Test.Fixtures;
using Mendham.Testing;
using Mendham.Testing.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
			Fixture.DomainEventHandlerContainer.AsMock()
				.Setup(a => a.HandleAllAsync(domainEvent))
				.ReturnsNoActionTask();

			var sut = Fixture.CreateSut();

			await sut.RaiseAsync(domainEvent);

			Fixture.DomainEventHandlerContainer.AsMock()
				.Verify(a => a.HandleAllAsync(domainEvent), Times.Once);
		}

		[Fact]
		public async Task RaiseAsync_DomainEvent_LogsEvent()
		{
			var domainEvent = Fixture.CreateDomainEvent();
            var logger = Mock.Of<IDomainEventLogger>();
            Fixture.DomainEventLoggers = logger.AsSingleItemEnumerable();
            Fixture.DomainEventHandlerContainer.AsMock()
				.Setup(a => a.HandleAllAsync(domainEvent))
				.ReturnsNoActionTask();

			var sut = Fixture.CreateSut();

			await sut.RaiseAsync(domainEvent);

            logger.AsMock()
                 .Verify(a => a.LogDomainEventRaised(domainEvent), Times.Once);
        }

		[Fact]
		public async Task RaiseAsync_HandlerException_StillLogsEvent()
		{
			var domainEvent = Fixture.CreateDomainEvent();
            var logger = Mock.Of<IDomainEventLogger>();
            Fixture.DomainEventLoggers = logger.AsSingleItemEnumerable();
            Fixture.DomainEventHandlerContainer.AsMock()
				.Setup(a => a.HandleAllAsync(domainEvent))
				.Throws<InvalidOperationException>();

			var sut = Fixture.CreateSut();

			await Assert.ThrowsAsync<InvalidOperationException>(
				() => sut.RaiseAsync(domainEvent));

            logger.AsMock()
                .Verify(a => a.LogDomainEventRaised(domainEvent), Times.Once); ;
        }
	}
}
