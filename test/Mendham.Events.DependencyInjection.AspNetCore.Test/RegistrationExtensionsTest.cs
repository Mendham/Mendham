using FluentAssertions;
using Mendham.Events.Components;
using Mendham.Events.DependencyInjection.TestObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Events.DependencyInjection.AspNetCore.Test
{
    public class RegistrationExtensionsTest
    {
        [Fact]
        public void AddEventHandling_GetEventPublisher_Resolves()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddEventHandling())
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var result = server.Host.Services.GetService<IEventPublisher>();

                result.Should()
                    .NotBeNull()
                    .And.BeOfType<EventPublisher>();
            }
        }

        [Fact]
        public void AddEventHandling_GetEventHandlingContainer_Resolves()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddEventHandling())
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var result = server.Host.Services.GetService<IEventHandlerContainer>();

                result.Should()
                    .NotBeNull()
                    .And.BeOfType<DefaultEventHandlerContainer>();
            }
        }

        [Fact]
        public void AddEventHandling_GetEventHandlingProcessor_Resolves()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddEventHandling())
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var result = server.Host.Services.GetService<IEventHandlerProcessor>();

                result.Should()
                    .NotBeNull()
                    .And.BeOfType<EventHandlerProcessor>();
            }
        }

        [Fact]
        public void AddEventHandling_GetEventLoggerProcessor_Resolves()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddEventHandling())
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var result = server.Host.Services.GetService<IEventLoggerProcessor>();

                result.Should()
                    .NotBeNull()
                    .And.BeOfType<EventLoggerProcessor>();
            }
        }

        [Fact]
        public void AddEventHandling_RegisterEventPublisherAfterOther_UsesOtherEventPublisher()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddTransient<IEventPublisher, AltEventPublisher>())
                .ConfigureServices(sc => sc.AddEventHandling())
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var result = server.Host.Services.GetService<IEventPublisher>();

                result.Should()
                    .NotBeNull()
                    .And.BeOfType<AltEventPublisher>();

                // verify the other services load as expected (ConfigureServices runs twice)
                var defaultEventProcessor = server.Host.Services.GetService<IEventLoggerProcessor>();
                defaultEventProcessor.Should()
                    .NotBeNull()
                    .And.BeOfType<EventLoggerProcessor>();
            }
        }
    }
}
