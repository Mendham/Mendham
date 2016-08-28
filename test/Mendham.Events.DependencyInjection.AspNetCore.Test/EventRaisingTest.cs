using FluentAssertions;
using Mendham.Events.DependencyInjection.TestObjects;
using Mendham.Events.DependencyInjection.TrackableTestObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Events.DependencyInjection.AspNetCore.Test
{
    public class EventRaisingTest
    {
        [Fact]
        public async Task Raise_SingleEvent_IsLogged()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(sc =>
                {
                    sc.AddEventHandling();
                    sc.AddSingleton<IEventLogger, TestEventLogger>();
                })
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var publisher = server.Host.Services.GetService<IEventPublisher>();
                var logger = server.Host.Services.GetService<IEventLogger>() as TestEventLogger;

                var domainEvent = new Test1Event();

                await publisher.RaiseAsync(domainEvent);

                logger.LoggedEvents.Should()
                    .HaveCount(1)
                    .And.Contain(domainEvent);
            }
        }

        [Fact]
        public async Task Raise_SingleHandler_IsRaised()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(sc =>
                {
                    sc.AddEventHandling()
                        .AddEventHandlers(typeof(WasCalledVerifiableEvent).GetTypeInfo().Assembly);
                    sc.AddSingleton<WasCalledTracker>();
                })
                .Configure(app => { });
            
            using (var server = new TestServer(builder))
            {
                var publisher = server.Host.Services.GetService<IEventPublisher>();
                var handler = server.Host.Services.GetService<IEnumerable<IEventHandler>>()
                    .OfType<WasCalledVerifiableHandler>()
                    .Single();

                var domainEvent = new WasCalledVerifiableEvent();

                await publisher.RaiseAsync(domainEvent);

                var result = handler.WasEverCalled;

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_SingleHandler_StartIsLogged()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(sc =>
                {
                    sc.AddEventHandling()
                        .AddEventHandlers(typeof(Test1EventHandler).GetTypeInfo().Assembly);
                    sc.AddSingleton<IEventHandlerLogger, VerifiableEventHandlerLogger<Test1EventHandler>>();
                })
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var publisher = server.Host.Services.GetService<IEventPublisher>();
                var handlerLogger = server.Host.Services.GetService<IEventHandlerLogger>() as IVerifiableEventHandlerLogger;

                var domainEvent = new Test1Event();

                await publisher.RaiseAsync(domainEvent);

                var result = handlerLogger.StartCalled;

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_SingleHandler_CompleteIsLogged()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(sc =>
                {
                    sc.AddEventHandling()
                        .AddEventHandlers(typeof(Test1EventHandler).GetTypeInfo().Assembly);
                    sc.AddSingleton<IEventHandlerLogger, VerifiableEventHandlerLogger<Test1EventHandler>>();
                })
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var publisher = server.Host.Services.GetService<IEventPublisher>();
                var handlerLogger = server.Host.Services.GetService<IEventHandlerLogger>() as IVerifiableEventHandlerLogger;

                var domainEvent = new Test1Event();

                await publisher.RaiseAsync(domainEvent);

                var result = handlerLogger.CompleteCalled;

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_HandlerRaisesSecondEvent_BothLogged()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(sc =>
                {
                    sc.AddEventHandling()
                        .AddEventHandlers(typeof(EventWithHandlerRegistered).GetTypeInfo().Assembly);
                    sc.AddSingleton<IEventLogger, TestEventLogger>();
                })
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var publisher = server.Host.Services.GetService<IEventPublisher>();
                var logger = server.Host.Services.GetService<IEventLogger>() as TestEventLogger;

                var originalDomainEvent = new EventWithHandlerRegistered();

                await publisher.RaiseAsync(originalDomainEvent);

                logger.LoggedEvents.Should()
                    .HaveCount(2)
                    .And.Contain(originalDomainEvent)
                    .And.Match(a => a.OfType<EventNoHandlerRegistered>().Any());
            }
        }
    }
}
