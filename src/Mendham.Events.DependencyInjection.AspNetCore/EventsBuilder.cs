using Microsoft.Extensions.DependencyInjection;

namespace Mendham.DependencyInjection.AspNetCore
{
    internal class EventsBuilder : IEventsBuilder
    {
        public IServiceCollection Services { get; }

        public EventsBuilder(IServiceCollection services)
        {
            Services = services
                .VerifyArgumentNotDefaultValue(nameof(services));
        }
    }
}
