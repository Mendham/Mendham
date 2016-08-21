using System;

namespace Mendham.Events
{
	public abstract class Event : IEvent
	{
        public Event() : this(DateTimeOffset.UtcNow)
        { }

		public Event(DateTimeOffset eventTime)
		{
			eventTime.VerifyArgumentNotDefaultValue(nameof(eventTime), "Event time is required");
			EventTime = eventTime;
		}

		public DateTimeOffset EventTime { get; protected set; }
	}
}
