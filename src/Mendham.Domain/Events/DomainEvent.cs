using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Events
{
	public abstract class DomainEvent : IDomainEvent
	{
		public DomainEvent()
		{
			this.TimeOccurred = DateTime.UtcNow;
		}

		public DomainEvent(DateTime timeOccured)
		{
			timeOccured.VerifyArgumentNotDefaultValue(nameof(timeOccured), "Event time is required");
			this.TimeOccurred = timeOccured;
		}

		public DateTime TimeOccurred { get; protected set; }
	}
}
