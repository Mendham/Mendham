using Mendham.Domain.Events;
using Mendham.Events;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.TestObjects
{
    public sealed class Test2DomainEventHandler : IEventHandler<Test2DomainEvent>
    {
        public Task HandleAsync(Test2DomainEvent domainEvent)
        {
            return Task.FromResult(0);
        }
    }

    public sealed class Test2DomainEvent : DomainEvent
    { }
}
