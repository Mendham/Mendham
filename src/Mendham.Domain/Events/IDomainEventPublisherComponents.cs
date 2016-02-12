namespace Mendham.Domain.Events
{
    public interface IDomainEventPublisherComponents
    {
        IDomainEventHandlerContainer DomainEventHandlerContainer { get; }
        IDomainEventLoggerContainer DomainEventLoggerContainer { get; }
    }
}