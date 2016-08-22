using Autofac;
using FluentAssertions;
using Mendham.Events.DependencyInjection.TestObjects;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Mendham.Events.DependencyInjection.Autofac.Test
{
    public class RegistrationExtensionsTest
    {
        [Fact]
        public void RegisterDomainEventHandlers_HandlersInAssembly_ReturnsAll()
        {
            var assembly = typeof(Test1EventHandler).GetTypeInfo().Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterEventHandlers(assembly);

            // This is needed one of the test handlersin the assembly (not used here) injects a publisher
            builder.RegisterModule<EventHandlingModule>();

            using (var sut = builder.Build().BeginLifetimeScope())
            {
                var result = sut.Resolve<IEnumerable<IEventHandler>>();

                result.Should()
                    .NotBeEmpty();
                result.Should()
                    .ContainItemsAssignableTo<IEventHandler>();
                result.Should()
                    .Contain(a => a is Test1EventHandler);
                result.Should()
                    .Contain(a => a is Test2EventHandler);
            }
        }
    }
}
