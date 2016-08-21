using System.Threading.Tasks;

namespace Mendham.Events
{
	/// <summary>
	/// Non generic handler used to register event handlers
	/// </summary>
	public interface IEventHandler
	{ }

	/// <summary>
	/// The interface for a event handler that handles the type of event as 
	/// well as any type derived from the event
	/// </summary>
	/// <typeparam name="TEvent">Type of event to be handled</typeparam>
	public interface IEventHandler<TEvent> : IEventHandler where TEvent : IEvent
	{
		/// <summary>
		/// Executes the Event Handler
		/// </summary>
		/// <param name="eventToHandle">The event to be handled.</param>
		/// <returns>A task that represents the completion of the event handler</returns>
		Task HandleAsync(TEvent eventToHandle);
	}
}