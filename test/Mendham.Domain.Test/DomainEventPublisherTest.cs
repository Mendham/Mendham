using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mendham.Domain.Events;
using Mendham.Domain.Test.Fixtures;
using Mendham.Testing;
using Moq;
using Xunit;

namespace Mendham.Domain.Test
{
	public class DomainEventPublisherTest : BaseUnitTest<DomainEventPublisherFixture>
	{
		public DomainEventPublisherTest(DomainEventPublisherFixture fixture) : base(fixture)
		{ }

		[Fact]
		public async Task RaiseAsync_DomainEvent_RaisesHandlers()
		{
			var domainEvent = _fixture.CreateDomainEvent();
			_fixture.DomainEventHandlerContainer.AsMock()
				.Setup(a => a.HandleAllAsync(domainEvent))
				.ReturnsNoActionTask();

			var sut = _fixture.CreateSut();

			await sut.RaiseAsync(domainEvent);

			_fixture.DomainEventHandlerContainer.AsMock()
				.Verify(a => a.HandleAllAsync(domainEvent), Times.Once);
		}

		[Fact]
		public async Task RaiseAsync_DomainEvent_LogsEvent()
		{
			var domainEvent = _fixture.CreateDomainEvent();
			var logger = Mock.Of<IDomainEventLogger>();
			_fixture.DomainEventLoggers = logger.AsSingleItemEnumerable();
			_fixture.DomainEventHandlerContainer.AsMock()
				.Setup(a => a.HandleAllAsync(domainEvent))
				.ReturnsNoActionTask();

			var sut = _fixture.CreateSut();

			await sut.RaiseAsync(domainEvent);

			logger.AsMock()
				.Verify(a => a.LogDomainEvent(domainEvent), Times.Once);
		}

		[Fact]
		public async Task RaiseAsync_HandlerException_StillLogsEvent()
		{
			var domainEvent = _fixture.CreateDomainEvent();
			var logger = Mock.Of<IDomainEventLogger>();
			_fixture.DomainEventLoggers = logger.AsSingleItemEnumerable();
			_fixture.DomainEventHandlerContainer.AsMock()
				.Setup(a => a.HandleAllAsync(domainEvent))
				.Throws<InvalidOperationException>();

			var sut = _fixture.CreateSut();

			await Assert.ThrowsAsync<InvalidOperationException>(
				() => sut.RaiseAsync(domainEvent));
			
			logger.AsMock()
				.Verify(a => a.LogDomainEvent(domainEvent), Times.Once);
		}
	}
}
