using System.Threading.Tasks;

namespace Mendham.Events
{
	/// <summary>
	/// Use this interface to raise events
	/// </summary>
	public interface IEventPublisher
	{
		/// <summary>
		/// Raises a event to be logged and handled
		/// </summary>
		/// <typeparam name="TEvent">Type of Event</typeparam>
		/// <param name="eventRaised">Event raised</param>
		/// <returns>Empty task after successfully raised</returns>
		/// <exception cref="EventHandlingException">One or more errors occured by handler</exception>
		Task RaiseAsync<TEvent>(TEvent eventRaised) where TEvent : class, IEvent;
	}
}
