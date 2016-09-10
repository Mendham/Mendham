using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

using Fixture = Mendham.Testing.Events.Test.EventPublisherFixtureTestingFixture;

namespace Mendham.Testing.Events.Test
{
    public class EventPublisherFixtureTest : Test<EventPublisherFixtureTestingFixture>
    {
        public EventPublisherFixtureTest(EventPublisherFixtureTestingFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task VerifyEventRaised_Raised_NoException()
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent1();

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent1>();

            act.ShouldNotThrow<EventVerificationException<Fixture.TestEvent1>>();
        }

        [Fact]
        public async Task VerifyEventRaised_RaisedTwice_NoException()
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent1();
            await Fixture.RaiseTestEvent1();

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent1>();

            act.ShouldNotThrow<EventVerificationException<Fixture.TestEvent1>>();
        }

        // Only running these tests in NET451 because Mendham.Testing.Builder does not work with Netstandard because of the
        // underlying dependencies. When those depdencies are upgraded to netstandard, then the if condition can be removed.
#if NET451

        [Theory, MendhamData]
        public void VerifyEventRaised_NotRaised_EventVerificationException(string userMessage)
        {
            var sut = Fixture.CreateSut();

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent1>(userMessage);

            act.ShouldThrow<EventVerificationException<Fixture.TestEvent1>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory, MendhamData]
        public async Task VerifyEventRaised_WrongEventRaised_EventVerificationException(string userMessage)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent1();

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent2>(userMessage);

            act.ShouldThrow<EventVerificationException<Fixture.TestEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Fact]
        public async Task VerifyEventRaisedTwice_RaisedTwice_NoException()
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent1();
            await Fixture.RaiseTestEvent1();

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent1>(TimesRaised.Exactly(2));

            act.ShouldNotThrow<EventVerificationException<Fixture.TestEvent1>>();
        }

        [Theory, MendhamData]
        public async Task VerifyEventRaisedTwice_RaiseOnce_EventVerificationException(string userMessage)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent1();

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent1>(TimesRaised.Exactly(2), userMessage);

            act.ShouldThrow<EventVerificationException<Fixture.TestEvent1>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory, MendhamData]
        public async Task VerifyEventRaisedWithCondition_Raised_NoException(string eventValue)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent2(eventValue);

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent2>(a => a.Value == eventValue);

            act.ShouldNotThrow<EventVerificationException<Fixture.TestEvent2>>();
        }

        [Theory, MendhamData]
        public async Task VerifyEventRaisedWithCondition_RaisedTwice_NoException(string eventValue)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent2(eventValue);
            await Fixture.RaiseTestEvent2(eventValue);

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent2>(a => a.Value == eventValue);

            act.ShouldNotThrow<EventVerificationException<Fixture.TestEvent2>>();
        }

        [Theory, MendhamData]
        public void VerifyEventRaisedWithCondition_NotRaised_EventVerificationException(string eventValue, string userMessage)
        {
            var sut = Fixture.CreateSut();

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent2>(a => a.Value == eventValue, userMessage);

            act.ShouldThrow<EventVerificationException<Fixture.TestEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory, MendhamData]
        public async Task VerifyEventRaisedWithCondition_RaisedIncorrectionCondition_EventVerificationException(
            string expectedEventValue, string actualEventValue, string userMessage)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent2(actualEventValue);

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent2>(a => a.Value == expectedEventValue, userMessage);

            act.ShouldThrow<EventVerificationException<Fixture.TestEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory, MendhamData]
        public async Task VerifyEventRaisedWithCondition_WrongEventRaised_EventVerificationException(string eventValue, string userMessage)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent1();

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent2>(a => a.Value == eventValue, userMessage);

            act.ShouldThrow<EventVerificationException<Fixture.TestEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory, MendhamData]
        public async Task VerifyEventRaisedWithConditionTwice_RaisedTwice_NoException(string eventValue)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent2(eventValue);
            await Fixture.RaiseTestEvent2(eventValue);

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent2>(
                a => a.Value == eventValue, TimesRaised.Exactly(2));

            act.ShouldNotThrow<EventVerificationException<Fixture.TestEvent2>>();
        }

        [Theory, MendhamData]
        public async Task VerifyEventRaisedWithConditionTwice_RaiseOnce_EventVerificationException(string eventValue, string userMessage)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent2(eventValue);

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent2>(
                a => a.Value == eventValue, TimesRaised.Exactly(2), userMessage);

            act.ShouldThrow<EventVerificationException<Fixture.TestEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory, MendhamData]
        public async Task VerifyEventRaisedWithConditionTwice_NotAllMeetCondition_EventVerificationException(
            string eventValue, string altEventValue, string userMessage)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent2(eventValue);
            await Fixture.RaiseTestEvent2(altEventValue);

            Action act = () => sut.VerifyEventRaised<Fixture.TestEvent2>(a => 
            a.Value == eventValue, TimesRaised.Exactly(2), userMessage);

            act.ShouldThrow<EventVerificationException<Fixture.TestEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }
#endif
    }
}
