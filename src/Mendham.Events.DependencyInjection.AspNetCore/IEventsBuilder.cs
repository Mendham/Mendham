using Microsoft.Extensions.DependencyInjection;

namespace Mendham.DependencyInjection.AspNetCore
{
    public interface IEventsBuilder
    {
        IServiceCollection Services { get; }
    }
}