using Microsoft.AspNetCore.Http.Features;

namespace Mendham.Testing.AspNetCore
{
    internal interface IWebHostFixture : IServiceProviderFixture
    {
        IFeatureCollection ServerFeatures { get; }
    }
}
