using Ploeh.AutoFixture.Kernel;
using System.Reflection;

namespace Mendham.Testing.Builder.AutoFixture
{
    internal static class MendhamSpecimenContextFactory
    {
        internal static IMendhamSpecimenContext CreateContext(this ISpecimenBuilder builder, 
            Assembly callingAssembly)
        {
            return new MendhamSpecimenContext(builder, callingAssembly);
        }
    }
}
