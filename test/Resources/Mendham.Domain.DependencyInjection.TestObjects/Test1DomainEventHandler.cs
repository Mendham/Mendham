using Mendham.Domain.Events;
using System.Threading.Tasks;

namespace Mendham.Domain.DependencyInjection.TestObjects
{
    public sealed class Test1DomainEventHandler : DomainEventHandler<Test1DomainEvent>
    {
        public override Task HandleAsync(Test1DomainEvent domainEvent)
        {
            return Task.FromResult(0);
        }
    }

    public sealed class Test1DomainEvent : DomainEvent
    { }
}
