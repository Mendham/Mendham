using Mendham.Events;
using System;
using System.Globalization;

namespace Mendham.Testing
{
    public class EventVerificationException<TEvent> : Exception
        where TEvent : IEvent
    {
        internal EventVerificationException(int timesCalled, TimesRaised timesExpected, string userMessage= null)
			: base(MessageText(typeof(TEvent), timesCalled, timesExpected, userMessage))
		{
            timesCalled.VerifyArgumentMeetsCriteria(a => a >= 0,
                nameof(timesCalled), "Times called must be a positive value");
            timesExpected.VerifyArgumentNotDefaultValue(nameof(timesExpected));

            TimesCalled = timesCalled;
            TimesExpected = timesExpected;
        }

        private static string MessageText(Type t, int timesCalled, TimesRaised timesExpected, string userMessage)
        {
            var msg = "{3} Domain Event {0} publish error. The event was expected be called {1}, but was called {2} time(s)";

            return string.Format(CultureInfo.CurrentCulture, msg, t.FullName,
                timesExpected.GetFailDetails(), timesCalled, FormatUserMessage(userMessage));
        }

        private static string FormatUserMessage(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
                return string.Empty;

            return string.Format("{0}\r\n", userMessage);
        }

        public Type EventType
        {
            get { return typeof(TEvent); }
        }

        public int TimesCalled { get; private set; }
        public TimesRaised TimesExpected { get; private set; }
    }
}
