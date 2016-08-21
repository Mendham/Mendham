﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using Fixture = Mendham.Testing.Events.Test.DomainEventPublisherFixtureTestingFixture;

namespace Mendham.Testing.Events.Test
{
    public class DomainEventPublisherFixtureTest : UnitTest<DomainEventPublisherFixtureTestingFixture>
    {
        public DomainEventPublisherFixtureTest(DomainEventPublisherFixtureTestingFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task VerifyDomainEventRaised_Raised_NoException()
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent1();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent1>();

            act.ShouldNotThrow<DomainEventVerificationException<Fixture.TestEvent1>>();
        }

        [Fact]
        public async Task VerifyDomainEventRaised_RaisedTwice_NoException()
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent1();
            await Fixture.RaiseTestEvent1();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent1>();

            act.ShouldNotThrow<DomainEventVerificationException<Fixture.TestEvent1>>();
        }

        // Only running these tests in NET451 because Mendham.Testing.Builder does not work with Netstandard because of the
        // underlying dependencies. When those depdencies are upgraded to netstandard, then the if condition can be removed.
#if NET451


        [Theory, MendhamData]
        public void VerifyDomainEventRaised_NotRaised_DomainEventVerificationException(string userMessage)
        {
            var sut = Fixture.CreateSut();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent1>(userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestEvent1>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory, MendhamData]
        public async Task VerifyDomainEventRaised_WrongEventRaised_DomainEventVerificationException(string userMessage)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent1();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent2>(userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Fact]
        public async Task VerifyDomainEventRaisedTwice_RaisedTwice_NoException()
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent1();
            await Fixture.RaiseTestEvent1();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent1>(TimesRaised.Exactly(2));

            act.ShouldNotThrow<DomainEventVerificationException<Fixture.TestEvent1>>();
        }

        [Theory, MendhamData]
        public async Task VerifyDomainEventRaisedTwice_RaiseOnce_DomainEventVerificationException(string userMessage)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent1();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent1>(TimesRaised.Exactly(2), userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestEvent1>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory, MendhamData]
        public async Task VerifyDomainEventRaisedWithCondition_Raised_NoException(string domainEventValue)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent2(domainEventValue);

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent2>(a => a.Value == domainEventValue);

            act.ShouldNotThrow<DomainEventVerificationException<Fixture.TestEvent2>>();
        }

        [Theory, MendhamData]
        public async Task VerifyDomainEventRaisedWithCondition_RaisedTwice_NoException(string domainEventValue)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent2(domainEventValue);
            await Fixture.RaiseTestEvent2(domainEventValue);

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent2>(a => a.Value == domainEventValue);

            act.ShouldNotThrow<DomainEventVerificationException<Fixture.TestEvent2>>();
        }

        [Theory, MendhamData]
        public void VerifyDomainEventRaisedWithCondition_NotRaised_DomainEventVerificationException(string domainEventValue, string userMessage)
        {
            var sut = Fixture.CreateSut();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent2>(a => a.Value == domainEventValue, userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory, MendhamData]
        public async Task VerifyDomainEventRaisedWithCondition_RaisedIncorrectionCondition_DomainEventVerificationException(
            string expectedDomainEventValue, string actualDomainEventValue, string userMessage)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent2(actualDomainEventValue);

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent2>(a => a.Value == expectedDomainEventValue, userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory, MendhamData]
        public async Task VerifyDomainEventRaisedWithCondition_WrongEventRaised_DomainEventVerificationException(string domainEventValue, string userMessage)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent1();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent2>(a => a.Value == domainEventValue, userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory, MendhamData]
        public async Task VerifyDomainEventRaisedWithConditionTwice_RaisedTwice_NoException(string domainEventValue)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent2(domainEventValue);
            await Fixture.RaiseTestEvent2(domainEventValue);

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent2>(
                a => a.Value == domainEventValue, TimesRaised.Exactly(2));

            act.ShouldNotThrow<DomainEventVerificationException<Fixture.TestEvent2>>();
        }

        [Theory, MendhamData]
        public async Task VerifyDomainEventRaisedWithConditionTwice_RaiseOnce_DomainEventVerificationException(string domainEventValue, string userMessage)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent2(domainEventValue);

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent2>(
                a => a.Value == domainEventValue, TimesRaised.Exactly(2), userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory, MendhamData]
        public async Task VerifyDomainEventRaisedWithConditionTwice_NotAllMeetCondition_DomainEventVerificationException(
            string domainEventValue, string altDomainEventValue, string userMessage)
        {
            var sut = Fixture.CreateSut();
            await Fixture.RaiseTestEvent2(domainEventValue);
            await Fixture.RaiseTestEvent2(altDomainEventValue);

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestEvent2>(a => 
            a.Value == domainEventValue, TimesRaised.Exactly(2), userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }
#endif
    }
}
