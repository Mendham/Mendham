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
    public class EventPublisherFixture : IFixture
    {
        private PublishedEvents _publishedEvents;

        public EventPublisherFixture()
        {
            _publishedEvents = new PublishedEvents();
        }

        /// <summary>
        /// Resets the events being tracked by the fixture
        /// </summary>
        public void ResetFixture()
        {
            _publishedEvents = new PublishedEvents();
        }

        /// <summary>
        /// Gets the <see cref="IEventPublisher"/> that employs this fixture
        /// </summary>
        /// <returns></returns>
        public IEventPublisher GetEventPublisher()
        {
            return new EventPublisher(_publishedEvents);
        }

        /// <summary>
        /// Verify that a event was raised at least one time
        /// </summary>
        /// <typeparam name="TEvent">The type of event that was raised</typeparam>
        /// <param name="userMessage">Message to display when the verification fails</param>
        public void VerifyEventRaised<TEvent>(string userMessage = null)
            where TEvent : IEvent
        {
            VerifyEventRaised<TEvent>(a => true, TimesRaised.AtLeastOnce, userMessage);
        }

        /// <summary>
        /// Verify that a event was raised
        /// </summary>
        /// <typeparam name="TEvent">The type of event that was raised</typeparam>
        /// <param name="timesRaised">Expected number of times the event was raised</param>
        /// <param name="userMessage">Message to display when the verification fails</param>
        public void VerifyEventRaised<TEvent>(TimesRaised timesRaised, string userMessage = null)
            where TEvent : IEvent
        {
            VerifyEventRaised<TEvent>(a => true, timesRaised, userMessage);
        }

        /// <summary>
        /// Verify that a event was raised where it meets a specific condition at least once
        /// </summary>
        /// <typeparam name="TEvent">The type of event that was raised</typeparam>
        /// <param name="predicate">Predicate that defines valide condition for event to be verified</param>
        /// <param name="userMessage">Message to display when the verification fails</param>
        public void VerifyEventRaised<TEvent>(Func<TEvent, bool> predicate, string userMessage = null)
            where TEvent : IEvent
        {
            VerifyEventRaised(predicate, TimesRaised.AtLeastOnce, userMessage);
        }

        /// <summary>
        /// Verify that a event was raised where it meets a specific condition
        /// </summary>
        /// <typeparam name="TEvent">The type of event that was raised</typeparam>
        /// <param name="predicate">Predicate that defines valide condition for event to be verified</param>
        /// <param name="timesRaised">Expected number of times the event was raised</param>
        /// <param name="userMessage">Message to display when the verification fails</param>
        public void VerifyEventRaised<TEvent>(Func<TEvent, bool> predicate, 
            TimesRaised timesRaised, string userMessage = null)
            where TEvent : IEvent
        {
            var evts = _publishedEvents.GetCapturedEvents()
                .OfType<TEvent>()
                .Where(predicate);

            var evtCount = evts.Count();

            if (!timesRaised.Validate(evtCount))
                throw new EventVerificationException<TEvent>(evtCount, timesRaised, userMessage);
        }

        private class EventPublisher : IEventPublisher
        {
            private readonly IEventLogger _publishedEvents;

            public EventPublisher(PublishedEvents publishedEvents)
            {
                _publishedEvents = publishedEvents;
            }

            public Task RaiseAsync<TEvent>(TEvent eventRaised)
                where TEvent : class, IEvent
            {
                _publishedEvents.LogEvent(eventRaised);
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

            void IEventLogger.LogEvent(IEvent eventRaised)
            {
                _capturedEvents.Add(eventRaised);
            }
        }
    }
}