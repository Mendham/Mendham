using Mendham.DependencyInjection.AspNetCore;
using Mendham.Events;
using Mendham.Events.Components;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RegistrationExtensions
    {
        public static IEventsBuilder AddEventHandling(this IServiceCollection services)
        {
            services.TryAddTransient<IEventHandlerContainer, DefaultEventHandlerContainer>();
            services.TryAddTransient<Func<IEnumerable<IEventHandler>>>(serviceProvider =>
                () => serviceProvider.GetServices<IEventHandler>());

            services.TryAddTransient<IEventLoggerProcessor, EventLoggerProcessor>();
            services.TryAddTransient<Func<IEnumerable<IEventLogger>>>(serviceProvider =>
                () => serviceProvider.GetServices<IEventLogger>());

            services.TryAddTransient<IEventHandlerProcessor, EventHandlerProcessor>();
            services.TryAddTransient<IEventPublisherComponents, EventPublisherComponents>();

            services.TryAddTransient<IEventPublisher, EventPublisher>();

            return new EventsBuilder(services);
        }

        public static IEventsBuilder AddEventHandlers(this IEventsBuilder builder, params Assembly[] assemblies)
        {
            Type eventHandlerInterface = typeof(IEventHandler);

            assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => eventHandlerInterface.IsAssignableFrom(type))
                .Select(knownEventHandler => builder.Services.AddTransient(eventHandlerInterface, knownEventHandler))
                .ToList();

            return builder;
        }
    }
}
