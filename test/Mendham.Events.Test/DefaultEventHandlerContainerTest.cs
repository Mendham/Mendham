using FluentAssertions;
using Mendham.Events.Components;
using Mendham.Events.Test.Fixtures;
using Mendham.Events.Test.TestObjects;
using Mendham.Testing;
using System.Linq;
using Xunit;

namespace Mendham.Events.Test
{
    public class DefaultEventHandlerContainerTest : Test<DefaultEventHandlerContainerFixture>
	{
		public DefaultEventHandlerContainerTest(DefaultEventHandlerContainerFixture fixture) : base(fixture)
		{ }

		[Fact]
		public void HandleAllAsync_BaseEvent_BaseEventHandlerOnly()
		{
			var sut = Fixture.CreateSut();

			var result = sut.GetHandlers<BaseEvent>();

            result.Should()
                .ContainSingle()
                .And.Contain(Fixture.BaseEventHandler);
		}

        [Fact]
        public void HandleAllAsync_DerivedEvent_TwoHandlers()
        {
            var sut = Fixture.CreateSut();

            var result = sut.GetHandlers<DerivedEvent>();

            result.Should()
                .HaveCount(2);
        }

        [Fact]
        public void HandleAllAsync_DerivedEvent_DerivedHandler()
        {
            var sut = Fixture.CreateSut();

            var result = sut.GetHandlers<DerivedEvent>();

            result.Should()
                .Contain(Fixture.DerivedEventHandler);
        }

        [Fact]
        public void HandleAllAsync_DerivedEvent_WrappedBaseHandler()
        {
            var sut = Fixture.CreateSut();

            var result = sut.GetHandlers<DerivedEvent>()
                .OfType<IEventHandlerWrapper>()
                .FirstOrDefault();

            result.Should()
                .NotBeNull();
            result.GetBaseHandlerType().Should()
                .Be(Fixture.BaseEventHandler.GetType());
        }
    }
}
