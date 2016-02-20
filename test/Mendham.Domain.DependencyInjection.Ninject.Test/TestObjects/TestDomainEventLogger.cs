﻿using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.Ninject.Test.TestObjects
{
    public class TestDomainEventLogger : IDomainEventLogger
    {
        private readonly List<IDomainEvent> _loggedEvents;

        public TestDomainEventLogger()
        {
            _loggedEvents = new List<IDomainEvent>();
        }

        public void LogDomainEventRaised(IDomainEvent domainEvent)
        {
            _loggedEvents.Add(domainEvent);
        }

        public List<IDomainEvent> LoggedEvents
        {
            get { return _loggedEvents.ToList(); }
        }
    }
}
