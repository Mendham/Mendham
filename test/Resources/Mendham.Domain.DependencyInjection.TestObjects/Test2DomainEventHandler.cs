using Mendham.Domain.Events;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.TestObjects
{
    public sealed class Test2DomainEventHandler : DomainEventHandler<Test2DomainEvent>
    {
        public override Task HandleAsync(Test2DomainEvent domainEvent)
        {
            return Task.FromResult(0);
        }
    }

    public sealed class Test2DomainEvent : DomainEvent
    { }
}
