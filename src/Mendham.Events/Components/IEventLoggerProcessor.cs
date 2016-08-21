namespace Mendham.Events.Components
{
    public interface IEventLoggerProcessor
    {
        void LogEvent(IEvent eventRaised);
    }
}
