using FluentAssertions;
using Mendham.DependencyInjection.Ninject;
using Mendham.Domain.DependencyInjection.ComplexDomainGraph;
using Mendham.Domain.DependencyInjection.Ninject.Test.TestObjects;
using Ninject;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.DependencyInjection.Ninject.Test
{
    public class ComplexGraphTest
    {
        [Fact]
        public async Task Raise_ComplexDomainGraph_ReturnsTrueAfterProcessing()
        {
            // The purpose of this test is to take an action on a complex graph to make sure the container 
            // does not throw a circular dependency error

            using (var kernel = new StandardKernel(new EventHandlingModule()))
            {
                kernel.RegisterEventHandlers(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);
                kernel.RegisterDomainFacades(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);
                kernel.Bind<IHasCircularHandlerService>().To<HasCircularHandlerService>();
                kernel.Bind<ICountService>().To<CountService>().InSingletonScope();
                kernel.Bind<IOtherService>().To<OtherService>();
                kernel.Bind<IEntityCreationService>().To<EntityCreationService>();
                kernel.Bind<IEntityFactory>().To<ComplexGraphEntityFactory>();

                var sut = kernel.Get<IHasCircularHandlerService>();

                var result = await sut.StartAsync();

                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Raise_ComplexDomainGraphMultiple_ReturnsTrueAfterProcessing()
        {
            // The purpose of this test is to take an action on a complex graph to make sure the container 
            // does not throw a circular dependency error

            using (var kernel = new StandardKernel(new EventHandlingModule()))
            {
                kernel.RegisterEventHandlers(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);
                kernel.RegisterDomainFacades(typeof(IHasCircularHandlerService).GetTypeInfo().Assembly);
                kernel.Bind<IHasCircularHandlerService>().To<HasCircularHandlerService>();
                kernel.Bind<ICountService>().To<CountService>().InSingletonScope();
                kernel.Bind<IOtherService>().To<OtherService>();
                kernel.Bind<IEntityCreationService>().To<EntityCreationService>();
                kernel.Bind<IEntityFactory>().To<ComplexGraphEntityFactory>();

                var sut = kernel.Get<IHasCircularHandlerService>();

                var result = await Task.WhenAll(Enumerable.Range(1, 10)
                    .Select(a => sut.StartAsync()).ToList());

                result.Should().NotBeEmpty()
                    .And.NotContain(false);
            }
        }
    }
}
