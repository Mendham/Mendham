using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Events.DependencyInjection.TestObjects
{
    public class AltEventPublisher : IEventPublisher
    {
        Task IEventPublisher.RaiseAsync<TEvent>(TEvent eventRaised)
        {
            throw new NotImplementedException();
        }
    }
}
