using Mendham.Events;

namespace Mendham.Domain.DependencyInjection.ComplexDomainGraph
{
    public interface IOtherService
    { }

    public class OtherService : IOtherService
    {
        private readonly IEventPublisher _eventPublisher;

        public OtherService(IEventPublisher eventPublishe)
        {
            _eventPublisher = eventPublishe;
        }
    }
}
