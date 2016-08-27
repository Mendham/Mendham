using Autofac;
using FluentAssertions;
using Mendham.DependencyInjection.Autofac;
using Mendham.Domain.DependencyInjection.Autofac.Test.TestObjects;
using Mendham.Domain.DependencyInjection.ComplexDomainGraph;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.DependencyInjection.Autofac.Test
{
    public class ComplexGraphTest
    {
        [Fact]
        public async Task Raise_ComplexDomainGraph_ReturnsTrueAfterProcessing()
        {
            // The purpose of this test is to take an action on a complex graph to make sure the container 
            // does not throw a circular dependency error

            var builder = new ContainerBuilder();

            builder.RegisterModule<EventHandlingModule>();
            builder.RegisterEventHandlers(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);
            builder.RegisterDomainFacades(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);
            builder.RegisterEntities(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);

            builder.RegisterType<HasCircularHandlerService>().As<IHasCircularHandlerService>();
            builder.RegisterType<CountService>().As<ICountService>().SingleInstance();
            builder.RegisterType<OtherService>().As<IOtherService>();
            builder.RegisterType<EntityCreationService>().As<IEntityCreationService>();
            builder.RegisterType<ComplexGraphEntityFactory>().As<IEntityFactory>();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var sut = scope.Resolve<IHasCircularHandlerService>();

                var result = await sut.StartAsync();

                result.Should().BeTrue();
            }
        }
    }
}
