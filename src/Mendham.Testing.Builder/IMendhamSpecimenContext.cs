using Ploeh.AutoFixture.Kernel;
using System.Reflection;

namespace Mendham.Testing.Builder.AutoFixture
{
    public interface IMendhamSpecimenContext : ISpecimenContext
    {
        Assembly CallingAssembly { get; }
    }
}