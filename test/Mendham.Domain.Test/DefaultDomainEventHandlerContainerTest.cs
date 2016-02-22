using FluentAssertions;
using Mendham.Domain.Events.Components;
using Mendham.Domain.Test.Fixtures;
using Mendham.Domain.Test.TestObjects.Events;
using Mendham.Testing;
using System.Linq;
using Xunit;

namespace Mendham.Domain.Test
{
    public class DefaultDomainEventHandlerContainerTest : UnitTest<DefaultDomainEventHandlerContainerFixture>
	{
		public DefaultDomainEventHandlerContainerTest(DefaultDomainEventHandlerContainerFixture fixture) : base(fixture)
		{ }

		[Fact]
		public void HandleAllAsync_BaseEvent_BaseEventHandlerOnly()
		{
			var sut = Fixture.CreateSut();

			var result = sut.GetHandlers<BaseDomainEvent>();

            result.Should()
                .ContainSingle()
                .And.Contain(Fixture.BaseEventHandler);
		}

        [Fact]
        public void HandleAllAsync_DerivedEvent_TwoHandlers()
        {
            var sut = Fixture.CreateSut();

            var result = sut.GetHandlers<DerivedDomainEvent>();

            result.Should()
                .HaveCount(2);
        }

        [Fact]
        public void HandleAllAsync_DerivedEvent_DerivedHandler()
        {
            var sut = Fixture.CreateSut();

            var result = sut.GetHandlers<DerivedDomainEvent>();

            result.Should()
                .Contain(Fixture.DerivedEventHandler);
        }

        [Fact]
        public void HandleAllAsync_DerivedEvent_WrappedBaseHandler()
        {
            var sut = Fixture.CreateSut();

            var result = sut.GetHandlers<DerivedDomainEvent>()
                .OfType<IDomainEventHandlerWrapper>()
                .FirstOrDefault();

            result.Should()
                .NotBeNull();
            result.GetBaseHandlerType().Should()
                .Be(Fixture.BaseEventHandler.GetType());
        }
    }
}
