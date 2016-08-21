using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Events
{
	/// <summary>
	/// Use this interface for any logger that should log when any domain event is raised
	/// </summary>
	public interface IEventLogger
	{
        /// <summary>
        /// Logs Domain Event
        /// </summary>
        /// <param name="eventRaised">Domain Event</param>
        void LogEvent(IEvent eventRaised);
	}
}
