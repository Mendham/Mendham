using System;

namespace Mendham.Testing.AspNetCore
{
    internal interface IServiceProviderFixture : IFixture, IDisposable
    {
        IServiceProvider Services { get; }
    }
}
