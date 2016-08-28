using Mendham.Events;
using Mendham.Events.Components;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RegistrationExtensions
    {
        public static void AddEventHandling(this IServiceCollection services)
        {
            services.AddTransient<IEventHandlerContainer, DefaultEventHandlerContainer>();
            services.AddTransient<Func<IEnumerable<IEventHandler>>>(serviceProvider =>
                () => serviceProvider.GetServices<IEventHandler>());

            services.AddTransient<IEventLoggerProcessor, EventLoggerProcessor>();
            services.AddTransient<Func<IEnumerable<IEventLogger>>>(serviceProvider =>
                () => serviceProvider.GetServices<IEventLogger>());

            services.AddTransient<IEventHandlerProcessor, EventHandlerProcessor>();

            services.AddTransient<IEventPublisherComponents, EventPublisherComponents>();

            services.AddTransient<IEventLoggerProcessor>(serviceProvider =>
                new EventLoggerProcessor(() => serviceProvider.GetServices<IEventLogger>()));

            services.AddTransient<IEventHandlerProcessor, EventHandlerProcessor>();

            services.TryAddTransient<IEventPublisher, EventPublisher>();
        }
    }
}
