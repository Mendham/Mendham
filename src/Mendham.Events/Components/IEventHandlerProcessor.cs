using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mendham.Events.Components
{
    public interface IEventHandlerProcessor
    {
        /// <summary>
        /// Asynchronously processes the event for each of the handlers
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventRaised">Raised event</param>
        /// <param name="handlers">All registered handlers for the event type</param>
		/// <exception cref="EventHandlingException">One or more errors occured by handler</exception>
        Task HandleAllAsync<TEvent>(TEvent eventRaised, IEnumerable<IEventHandler<TEvent>> handlers) where TEvent : IEvent;
    }
}
