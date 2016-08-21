namespace Mendham.Events.Components
{
    public interface IEventPublisherComponents
    {
        IEventHandlerContainer EventHandlerContainer { get; }
        IEventHandlerProcessor EventHandlerProcessor { get; }
        IEventLoggerProcessor EventLoggerProcessor { get; }
    }
}
