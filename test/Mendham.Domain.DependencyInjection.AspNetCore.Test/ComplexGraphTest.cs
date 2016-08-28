using FluentAssertions;
using Mendham.Domain.DependencyInjection.AspNetCore.Test.TestObjects;
using Mendham.Domain.DependencyInjection.ComplexDomainGraph;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Domain.DependencyInjection.AspNetCore.Test
{
    public class ComplexGraphTest
    {
        [Fact]
        public async Task Raise_ComplexDomainGraph_ReturnsTrueAfterProcessing()
        {
            // The purpose of this test is to take an action on a complex graph to make sure the container 
            // does not throw a circular dependency error

            var builder = new WebHostBuilder()
                .ConfigureServices(sc =>
                {
                    sc.AddDomain(typeof(Entity1).GetTypeInfo().Assembly);
                    sc.AddTransient<IHasCircularHandlerService, HasCircularHandlerService>();
                    sc.AddSingleton<ICountService, CountService>();
                    sc.AddTransient<IOtherService, OtherService>();
                    sc.AddTransient<IEntityCreationService, EntityCreationService>();
                    sc.AddTransient<IEntityFactory, ComplexGraphEntityFactory>();
                })
                .Configure(app => { });

            using (var server = new TestServer(builder))
            {
                var sut = server.Host.Services.GetService<IHasCircularHandlerService>();

                var result = await sut.StartAsync();

                result.Should().BeTrue();
            }
        }
    }
}
