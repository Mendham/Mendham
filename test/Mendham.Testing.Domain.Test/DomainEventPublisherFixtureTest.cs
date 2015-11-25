﻿using FluentAssertions;
using Mendham.Domain.Events;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using Fixture = Mendham.Testing.Domain.Test.DomainEventPublisherFixtureTestingFixture;

namespace Mendham.Testing.Domain.Test
{
    public class DomainEventPublisherFixtureTest : BaseUnitTest<DomainEventPublisherFixtureTestingFixture>
    {
        public DomainEventPublisherFixtureTest(DomainEventPublisherFixtureTestingFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task VerifyDomainEventRaised_Raised_NoException()
        {
            var sut = TestFixture.CreateSut();
            await TestFixture.RaiseTestDomainEvent1();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent1>();

            act.ShouldNotThrow<DomainEventVerificationException<Fixture.TestDomainEvent1>>();
        }

        [Fact]
        public async Task VerifyDomainEventRaised_RaisedTwice_NoException()
        {
            var sut = TestFixture.CreateSut();
            await TestFixture.RaiseTestDomainEvent1();
            await TestFixture.RaiseTestDomainEvent1();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent1>();

            act.ShouldNotThrow<DomainEventVerificationException<Fixture.TestDomainEvent1>>();
        }

        [Theory]
        [AutoData]
        public void VerifyDomainEventRaised_NotRaised_DomainEventVerificationException(string userMessage)
        {
            var sut = TestFixture.CreateSut();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent1>(userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestDomainEvent1>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory]
        [AutoData]
        public async Task VerifyDomainEventRaised_WrongEventRaised_DomainEventVerificationException(string userMessage)
        {
            var sut = TestFixture.CreateSut();
            await TestFixture.RaiseTestDomainEvent1();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent2>(userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestDomainEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Fact]
        public async Task VerifyDomainEventRaisedTwice_RaisedTwice_NoException()
        {
            var sut = TestFixture.CreateSut();
            await TestFixture.RaiseTestDomainEvent1();
            await TestFixture.RaiseTestDomainEvent1();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent1>(TimesRaised.Exactly(2));

            act.ShouldNotThrow<DomainEventVerificationException<Fixture.TestDomainEvent1>>();
        }

        [Theory]
        [AutoData]
        public async Task VerifyDomainEventRaisedTwice_RaiseOnce_DomainEventVerificationException(string userMessage)
        {
            var sut = TestFixture.CreateSut();
            await TestFixture.RaiseTestDomainEvent1();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent1>(TimesRaised.Exactly(2), userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestDomainEvent1>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory]
        [AutoData]
        public async Task VerifyDomainEventRaisedWithCondition_Raised_NoException(string domainEventValue)
        {
            var sut = TestFixture.CreateSut();
            await TestFixture.RaiseTestDomainEvent2(domainEventValue);

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent2>(a => a.Value == domainEventValue);

            act.ShouldNotThrow<DomainEventVerificationException<Fixture.TestDomainEvent2>>();
        }

        [Theory]
        [AutoData]
        public async Task VerifyDomainEventRaisedWithCondition_RaisedTwice_NoException(string domainEventValue)
        {
            var sut = TestFixture.CreateSut();
            await TestFixture.RaiseTestDomainEvent2(domainEventValue);
            await TestFixture.RaiseTestDomainEvent2(domainEventValue);

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent2>(a => a.Value == domainEventValue);

            act.ShouldNotThrow<DomainEventVerificationException<Fixture.TestDomainEvent2>>();
        }

        [Theory]
        [AutoData]
        public void VerifyDomainEventRaisedWithCondition_NotRaised_DomainEventVerificationException(string domainEventValue, string userMessage)
        {
            var sut = TestFixture.CreateSut();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent2>(a => a.Value == domainEventValue, userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestDomainEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory]
        [AutoData]
        public async Task VerifyDomainEventRaisedWithCondition_RaisedIncorrectionCondition_DomainEventVerificationException(
            string expectedDomainEventValue, string actualDomainEventValue, string userMessage)
        {
            var sut = TestFixture.CreateSut();
            await TestFixture.RaiseTestDomainEvent2(actualDomainEventValue);

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent2>(a => a.Value == expectedDomainEventValue, userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestDomainEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory]
        [AutoData]
        public async Task VerifyDomainEventRaisedWithCondition_WrongEventRaised_DomainEventVerificationException(string domainEventValue, string userMessage)
        {
            var sut = TestFixture.CreateSut();
            await TestFixture.RaiseTestDomainEvent1();

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent2>(a => a.Value == domainEventValue, userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestDomainEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory]
        [AutoData]
        public async Task VerifyDomainEventRaisedWithConditionTwice_RaisedTwice_NoException(string domainEventValue)
        {
            var sut = TestFixture.CreateSut();
            await TestFixture.RaiseTestDomainEvent2(domainEventValue);
            await TestFixture.RaiseTestDomainEvent2(domainEventValue);

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent2>(
                a => a.Value == domainEventValue, TimesRaised.Exactly(2));

            act.ShouldNotThrow<DomainEventVerificationException<Fixture.TestDomainEvent2>>();
        }

        [Theory]
        [AutoData]
        public async Task VerifyDomainEventRaisedWithConditionTwice_RaiseOnce_DomainEventVerificationException(string domainEventValue, string userMessage)
        {
            var sut = TestFixture.CreateSut();
            await TestFixture.RaiseTestDomainEvent2(domainEventValue);

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent2>(
                a => a.Value == domainEventValue, TimesRaised.Exactly(2), userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestDomainEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }

        [Theory]
        [AutoData]
        public async Task VerifyDomainEventRaisedWithConditionTwice_NotAllMeetCondition_DomainEventVerificationException(
            string domainEventValue, string altDomainEventValue, string userMessage)
        {
            var sut = TestFixture.CreateSut();
            await TestFixture.RaiseTestDomainEvent2(domainEventValue);
            await TestFixture.RaiseTestDomainEvent2(altDomainEventValue);

            Action act = () => sut.VerifyDomainEventRaised<Fixture.TestDomainEvent2>(a => 
            a.Value == domainEventValue, TimesRaised.Exactly(2), userMessage);

            act.ShouldThrow<DomainEventVerificationException<Fixture.TestDomainEvent2>>()
                .Where(a => a.Message.Contains(userMessage));
        }
    }
}