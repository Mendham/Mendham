using Ploeh.AutoFixture.Kernel;
using System.Reflection;

namespace Mendham.Testing.Builder.AutoFixture
{
    internal interface IMendhamSpecimenContext : ISpecimenContext
    {
        Assembly CallingAssembly { get; }
    }
}