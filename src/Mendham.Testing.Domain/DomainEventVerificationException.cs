using Mendham.Domain.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    public class DomainEventVerificationException<TDomainEvent> : Exception
        where TDomainEvent : IDomainEvent
    {
        internal DomainEventVerificationException(int timesCalled, TimesRaised timesExpected, string userMessage= null)
			: base(MessageText(typeof(TDomainEvent), timesCalled, timesExpected, userMessage))
		{
            timesCalled.VerifyArgumentMeetsCriteria(a => a >= 0, "Times called must be a positive value");
            timesExpected.VerifyArgumentNotDefaultValue("TimesRaised is required");

            this.TimesCalled = timesCalled;
            this.TimesExpected = timesExpected;
        }

        private static string MessageText(Type t, int timesCalled, TimesRaised timesExpected, string userMessage)
        {
            var msg = "{3}Domain Event {0} publish error. The event was expected be called {1}, but was called {2} time(s)";

            return string.Format(CultureInfo.CurrentCulture, msg, t.FullName,
                timesExpected.GetFailDetails(), timesCalled, FormatUserMessage(userMessage));
        }

        private static string FormatUserMessage(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
                return string.Empty;

            return string.Format("{0}\r\n", userMessage);
        }

        public Type DomainEventType
        {
            get { return typeof(TDomainEvent); }
        }

        public int TimesCalled { get; private set; }
        public TimesRaised TimesExpected { get; private set; }
    }
}
