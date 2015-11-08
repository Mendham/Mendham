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
			var domainEvent = TestFixture.CreateDomainEvent();
			TestFixture.DomainEventHandlerContainer.AsMock()
				.Setup(a => a.HandleAllAsync(domainEvent))
				.ReturnsNoActionTask();

			var sut = TestFixture.CreateSut();

			await sut.RaiseAsync(domainEvent);

			TestFixture.DomainEventHandlerContainer.AsMock()
				.Verify(a => a.HandleAllAsync(domainEvent), Times.Once);
		}

		[Fact]
		public async Task RaiseAsync_DomainEvent_LogsEvent()
		{
			var domainEvent = TestFixture.CreateDomainEvent();
			var logger = Mock.Of<IDomainEventLogger>();
			TestFixture.DomainEventLoggers = logger.AsSingleItemEnumerable();
			TestFixture.DomainEventHandlerContainer.AsMock()
				.Setup(a => a.HandleAllAsync(domainEvent))
				.ReturnsNoActionTask();

			var sut = TestFixture.CreateSut();

			await sut.RaiseAsync(domainEvent);

			logger.AsMock()
				.Verify(a => a.LogDomainEvent(domainEvent), Times.Once);
		}

		[Fact]
		public async Task RaiseAsync_HandlerException_StillLogsEvent()
		{
			var domainEvent = TestFixture.CreateDomainEvent();
			var logger = Mock.Of<IDomainEventLogger>();
			TestFixture.DomainEventLoggers = logger.AsSingleItemEnumerable();
			TestFixture.DomainEventHandlerContainer.AsMock()
				.Setup(a => a.HandleAllAsync(domainEvent))
				.Throws<InvalidOperationException>();

			var sut = TestFixture.CreateSut();

			await Assert.ThrowsAsync<InvalidOperationException>(
				() => sut.RaiseAsync(domainEvent));
			
			logger.AsMock()
				.Verify(a => a.LogDomainEvent(domainEvent), Times.Once);
		}
	}
}
