using Mendham.Domain.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    public class DomainEventPublisherFixture : IFixture
    {
        private PublishedEvents publishedEvents;

        public DomainEventPublisherFixture()
        {
            this.publishedEvents = new PublishedEvents();
        }

        public void ResetFixture()
        {
            this.publishedEvents = new PublishedEvents();
        }

        public IDomainEventPublisherProvider GetDomainEventPublisherProvider()
        {
            return new DomainEventPublisherProvider(this.publishedEvents);
        }

        public void VerifyDomainEventRaised<TDomainEvent>(int? timesRaised = 1)
            where TDomainEvent : IDomainEvent
        {
            VerifyDomainEventRaised<TDomainEvent>(a => true, timesRaised);
        }

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