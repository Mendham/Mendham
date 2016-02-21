using Mendham.Domain.Events;

namespace Mendham.Domain.DependencyInjection.ComplexDomainGraph
{
    public interface IOtherService
    { }

    public class OtherService : IOtherService
    {
        private readonly IDomainEventPublisher domainEventPublisher;

        public OtherService(IDomainEventPublisher domainEventPublisher)
        {
            this.domainEventPublisher = domainEventPublisher;
        }
    }
}
