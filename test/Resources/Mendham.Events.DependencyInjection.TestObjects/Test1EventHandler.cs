using Mendham.Events;
using System.Threading.Tasks;

namespace Mendham.Events.DependencyInjection.TestObjects
{
    public sealed class Test1EventHandler : IEventHandler<Test1Event>
    {
        public Task HandleAsync(Test1Event domainEvent)
        {
            return Task.FromResult(0);
        }
    }

    public sealed class Test1Event : Event
    { }
}
