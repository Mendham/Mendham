using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mendham.Testing.AspNetCore.Test.SampleApp
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddJsonFormatters();

            services.TryAddTransient<ITestService, TestService>();
            services.TryAddTransient<IDependency1, Dependency1>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
