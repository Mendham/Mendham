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
        /// Gets the IDomainEventPublisher that employs this fixture
        /// </summary>
        /// <returns></returns>
        public IDomainEventPublisher GetDomainEventPublisher()
        {
            return new DomainEventPublisher(this.publishedEvents);
        }

        /// <summary>
        /// Verify that a domain event was raised at least one time
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of domain event that was raised</typeparam>
        /// <param name="userMessage">Message to display when the verification fails</param>
        public void VerifyDomainEventRaised<TDomainEvent>(string userMessage = null)
            where TDomainEvent : IDomainEvent
        {
            VerifyDomainEventRaised<TDomainEvent>(a => true, TimesRaised.AtLeastOnce, userMessage);
        }

        /// <summary>
        /// Verify that a domain event was raised
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of domain event that was raised</typeparam>
        /// <param name="timesRaised">Expected number of times the domain event was raised</param>
        /// <param name="userMessage">Message to display when the verification fails</param>
        public void VerifyDomainEventRaised<TDomainEvent>(TimesRaised timesRaised, string userMessage = null)
            where TDomainEvent : IDomainEvent
        {
            VerifyDomainEventRaised<TDomainEvent>(a => true, timesRaised, userMessage);
        }

        /// <summary>
        /// Verify that a domain event was raised where it meets a specific condition at least once
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of domain event that was raised</typeparam>
        /// <param name="predicate">Predicate that defines valide condition for domain event to be verified</param>
        /// <param name="userMessage">Message to display when the verification fails</param>
        public void VerifyDomainEventRaised<TDomainEvent>(Func<TDomainEvent, bool> predicate, string userMessage = null)
            where TDomainEvent : IDomainEvent
        {
            VerifyDomainEventRaised(predicate, TimesRaised.AtLeastOnce, userMessage);
        }

        /// <summary>
        /// Verify that a domain event was raised where it meets a specific condition
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of domain event that was raised</typeparam>
        /// <param name="predicate">Predicate that defines valide condition for domain event to be verified</param>
        /// <param name="timesRaised">Expected number of times the domain event was raised</param>
        /// <param name="userMessage">Message to display when the verification fails</param>
        public void VerifyDomainEventRaised<TDomainEvent>(Func<TDomainEvent, bool> predicate, 
            TimesRaised timesRaised, string userMessage = null)
            where TDomainEvent : IDomainEvent
        {
            var evts = publishedEvents.GetCapturedEvents()
                .OfType<TDomainEvent>()
                .Where(predicate);

            var evtCount = evts.Count();

            if (!timesRaised.Validate(evtCount))
                throw new DomainEventVerificationException<TDomainEvent>(evtCount, timesRaised, userMessage);
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