using Mendham.Events;
using System.Threading.Tasks;

namespace Mendham.Events.DependencyInjection.TestObjects
{
    public sealed class Test2EventHandler : IEventHandler<Test2Event>
    {
        public Task HandleAsync(Test2Event domainEvent)
        {
            return Task.FromResult(0);
        }
    }

    public sealed class Test2Event : Event
    { }
}
