using System;

namespace Mendham.Events
{
	/// <summary>
	/// An application event
	/// </summary>
	public interface IEvent
	{
		/// <summary>
		/// Time in which the event occured
		/// </summary>
		DateTimeOffset EventTime { get; }
	}
}
