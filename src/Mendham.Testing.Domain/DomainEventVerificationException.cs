using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    public class DomainEventVerificationException<TDomainEvent> : Exception
        where TDomainEvent : IDomainEvent
    {
        internal DomainEventVerificationException(int timesCalled, int? timesExpected = null)
			: base(MessageText(typeof(TDomainEvent), timesCalled, timesExpected))
		{
            this.TimesCalled = timesCalled;
            this.TimesExpected = timesExpected;
        }

        private static string MessageText(Type t, int timesCalled, int? timesExpected)
        {
            var msg = "Domain Event {0} publish error. Expected {1}, Actual {2}";
            var expected = timesExpected.HasValue ? timesExpected.ToString() : ">=1";

            return string.Format(msg, t.Name, expected, timesCalled);
        }

        public Type DomainEventType
        {
            get { return typeof(TDomainEvent); }
        }

        public int TimesCalled { get; private set; }
        public int? TimesExpected { get; private set; }
    }
}
