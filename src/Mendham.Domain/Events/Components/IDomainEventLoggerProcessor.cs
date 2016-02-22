namespace Mendham.Domain.Events.Components
{
    public interface IDomainEventLoggerProcessor
    {
        void LogDomainEvent(IDomainEvent domainEvent);
    }
}
