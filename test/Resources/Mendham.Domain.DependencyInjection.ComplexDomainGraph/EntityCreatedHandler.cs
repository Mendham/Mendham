using Mendham.Domain.Events;
using Mendham.Events;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.ComplexDomainGraph
{
    public class EntityCreatedHandler : IEventHandler<EntityCreated>
    {
        public Task HandleAsync(EntityCreated domainEvent)
        {
            return Task.FromResult(0);
        }
    }

    public class EntityCreated : DomainEvent
    {
    }
}
