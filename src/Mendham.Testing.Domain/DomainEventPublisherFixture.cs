using Mendham.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    /// <summary>
    /// Fixture used for testing event publishing
    /// </summary>
    public class DomainEventPublisherFixture : IFixture
    {
        private PublishedEvents _publishedEvents;

        public DomainEventPublisherFixture()
        {
            _publishedEvents = new PublishedEvents();
        }

        /// <summary>
        /// Resets the domain events being tracked by the fixture
        /// </summary>
        public void ResetFixture()
        {
            _publishedEvents = new PublishedEvents();
        }

        /// <summary>
        /// Gets the IDomainEventPublisher that employs this fixture
        /// </summary>
        /// <returns></returns>
        public IEventPublisher GetDomainEventPublisher()
        {
            return new DomainEventPublisher(_publishedEvents);
        }

        /// <summary>
        /// Verify that a domain event was raised at least one time
        /// </summary>
        /// <typeparam name="TEvent">The type of domain event that was raised</typeparam>
        /// <param name="userMessage">Message to display when the verification fails</param>
        public void VerifyDomainEventRaised<TEvent>(string userMessage = null)
            where TEvent : IEvent
        {
            VerifyDomainEventRaised<TEvent>(a => true, TimesRaised.AtLeastOnce, userMessage);
        }

        /// <summary>
        /// Verify that a domain event was raised
        /// </summary>
        /// <typeparam name="TEvent">The type of domain event that was raised</typeparam>
        /// <param name="timesRaised">Expected number of times the domain event was raised</param>
        /// <param name="userMessage">Message to display when the verification fails</param>
        public void VerifyDomainEventRaised<TEvent>(TimesRaised timesRaised, string userMessage = null)
            where TEvent : IEvent
        {
            VerifyDomainEventRaised<TEvent>(a => true, timesRaised, userMessage);
        }

        /// <summary>
        /// Verify that a domain event was raised where it meets a specific condition at least once
        /// </summary>
        /// <typeparam name="TEvent">The type of domain event that was raised</typeparam>
        /// <param name="predicate">Predicate that defines valide condition for domain event to be verified</param>
        /// <param name="userMessage">Message to display when the verification fails</param>
        public void VerifyDomainEventRaised<TEvent>(Func<TEvent, bool> predicate, string userMessage = null)
            where TEvent : IEvent
        {
            VerifyDomainEventRaised(predicate, TimesRaised.AtLeastOnce, userMessage);
        }

        /// <summary>
        /// Verify that a domain event was raised where it meets a specific condition
        /// </summary>
        /// <typeparam name="TEvent">The type of domain event that was raised</typeparam>
        /// <param name="predicate">Predicate that defines valide condition for domain event to be verified</param>
        /// <param name="timesRaised">Expected number of times the domain event was raised</param>
        /// <param name="userMessage">Message to display when the verification fails</param>
        public void VerifyDomainEventRaised<TEvent>(Func<TEvent, bool> predicate, 
            TimesRaised timesRaised, string userMessage = null)
            where TEvent : IEvent
        {
            var evts = _publishedEvents.GetCapturedEvents()
                .OfType<TEvent>()
                .Where(predicate);

            var evtCount = evts.Count();

            if (!timesRaised.Validate(evtCount))
                throw new DomainEventVerificationException<TEvent>(evtCount, timesRaised, userMessage);
        }

        private class DomainEventPublisher : IEventPublisher
        {
            private readonly IEventLogger _publishedEvents;

            public DomainEventPublisher(PublishedEvents publishedEvents)
            {
                _publishedEvents = publishedEvents;
            }

            public Task RaiseAsync<TDomainEvent>(TDomainEvent domainEvent)
                where TDomainEvent : class, IEvent
            {
                _publishedEvents.LogEvent(domainEvent);
                return Task.FromResult(0);
            }
        }

        private class PublishedEvents : IEventLogger
        {
            private readonly BlockingCollection<IEvent> _capturedEvents = new BlockingCollection<IEvent>();

            internal IEnumerable<IEvent> GetCapturedEvents()
            {
                return _capturedEvents;
            }

            void IEventLogger.LogEvent(IEvent domainEvent)
            {
                _capturedEvents.Add(domainEvent);
            }
        }
    }
}