using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.AutoFixture
{
    public static class MendhamSpecimenContextFactory
    {
        public static IMendhamSpecimenContext CreateContext(this ISpecimenBuilder builder, 
            Assembly callingAssembly)
        {
            return new MendhamSpecimenContext(builder, callingAssembly);
        }
    }
}
