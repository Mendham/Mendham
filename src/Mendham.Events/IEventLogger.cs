using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Events
{
	/// <summary>
	/// Use this interface for any logger that should log when any event is raised
	/// </summary>
	public interface IEventLogger
	{
        /// <summary>
        /// Logs Event
        /// </summary>
        /// <param name="eventRaised">Event that was raised</param>
        void LogEvent(IEvent eventRaised);
	}
}
