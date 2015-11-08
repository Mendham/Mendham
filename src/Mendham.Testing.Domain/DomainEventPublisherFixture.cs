using Mendham.Domain.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    /// <summary>
    /// Fixture used for testing domain event publishing
    /// </summary>
    public class DomainEventPublisherFixture : IFixture
    {
        private PublishedEvents publishedEvents;

        public DomainEventPublisherFixture()
        {
            this.publishedEvents = new PublishedEvents();
        }

        /// <summary>
        /// Resets the domain events being tracked by the fixture
        /// </summary>
        public void ResetFixture()
        {
            this.publishedEvents = new PublishedEvents();
        }

        /// <summary>
        /// Gets the IDomainEventPublisherProvider that employs this fixture
        /// </summary>
        /// <returns></returns>
        public IDomainEventPublisherProvider GetDomainEventPublisherProvider()
        {
            return new DomainEventPublisherProvider(this.publishedEvents);
        }

        /// <summary>
        /// Verify that a domain event was raised
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of domain event that was raised</typeparam>
        /// <param name="timesRaised">Expected number of times the domain event was raised (default 1)</param>
        public void VerifyDomainEventRaised<TDomainEvent>(int? timesRaised = 1)
            where TDomainEvent : IDomainEvent
        {
            VerifyDomainEventRaised<TDomainEvent>(a => true, timesRaised);
        }

        /// <summary>
        /// Verify that a domain event was raised where it meets a specific condition
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of domain event that was raised</typeparam>
        /// <param name="predicate">Predicate that defines valide condition for domain event to be verified</param>
        /// <param name="timesRaised">Expected number of times the domain event was raised (default 1)</param>
        public void VerifyDomainEventRaised<TDomainEvent>(Func<TDomainEvent, bool> predicate, int? timesRaised = 1)
            where TDomainEvent : IDomainEvent
        {
            var evts = publishedEvents.GetCapturedEvents()
                .OfType<TDomainEvent>()
                .Where(predicate);

            if (!timesRaised.HasValue && !evts.Any())
                throw new DomainEventVerificationException<TDomainEvent>(0);
            else if (timesRaised.HasValue && evts.Count() != timesRaised.Value)
                throw new DomainEventVerificationException<TDomainEvent>(evts.Count(), timesRaised.Value);
        }

        private class DomainEventPublisherProvider : IDomainEventPublisherProvider
        {
            private readonly IDomainEventLogger publishedEvents;

            public DomainEventPublisherProvider(PublishedEvents publishedEvents)
            {
                this.publishedEvents = publishedEvents;
            }

            public IDomainEventPublisher GetPublisher()
            {
                return new DomainEventPublisher(publishedEvents as PublishedEvents);
            }
        }

        private class DomainEventPublisher : IDomainEventPublisher
        {
            private readonly IDomainEventLogger publishedEvents;

            public DomainEventPublisher(PublishedEvents publishedEvents)
            {
                this.publishedEvents = publishedEvents;
            }

            public Task RaiseAsync<TDomainEvent>(TDomainEvent domainEvent)
                where TDomainEvent : class, IDomainEvent
            {
                publishedEvents.LogDomainEvent(domainEvent);
                return Task.FromResult(0);
            }
        }

        private class PublishedEvents : IDomainEventLogger
        {
            private readonly BlockingCollection<IDomainEvent> CapturedEvents = new BlockingCollection<IDomainEvent>();

            internal IEnumerable<IDomainEvent> GetCapturedEvents()
            {
                return CapturedEvents;
            }

            void IDomainEventLogger.LogDomainEvent<TDomainEvent>(TDomainEvent domainEvent)
            {
                CapturedEvents.Add(domainEvent);
            }
        }
    }
}