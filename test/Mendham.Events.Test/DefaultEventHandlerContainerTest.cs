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
		public void GetHandlers_BaseEvent_BaseEventHandlerOnly()
		{
			var sut = Fixture.CreateSut();

			var result = sut.GetHandlers<BaseEvent>();

            result.Should()
                .ContainSingle()
                .And.Contain(Fixture.BaseEventHandler);
		}

        [Fact]
        public void GetHandlers_DerivedEvent_TwoHandlers()
        {
            var sut = Fixture.CreateSut();

            var result = sut.GetHandlers<DerivedEvent>();

            result.Should()
                .HaveCount(2);
        }

        [Fact]
        public void GetHandlers_DerivedEvent_DerivedHandler()
        {
            var sut = Fixture.CreateSut();

            var result = sut.GetHandlers<DerivedEvent>();

            result.Should()
                .Contain(Fixture.DerivedEventHandler);
        }

        [Fact]
        public void GetHandlers_DerivedEvent_WrappedBaseHandler()
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

        [Fact]
        public void GetHandlers_HandlerWithMultipleInterfaces_AllEventsReturned()
        {
            var sut = Fixture.CreateSut();

            var result1 = sut.GetHandlers<SharedEvent1>();
            var result2 = sut.GetHandlers<SharedEvent2>();

            result1.Should()
                .NotBeEmpty("SharedEventHandler implements IEventHandler<SharedEvent1>")
                .And.Contain(Fixture.SharedEventHandler as IEventHandler<SharedEvent1>);
            result2.Should()
                .NotBeEmpty("SharedEventHandler implements IEventHandler<SharedEvent1>")
                .And.Contain(Fixture.SharedEventHandler as IEventHandler<SharedEvent2>);
        }
    }
}
