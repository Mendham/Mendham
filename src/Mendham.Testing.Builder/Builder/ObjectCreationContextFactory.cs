using Mendham.Testing.Builder.AutoFixture;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    internal static class ObjectCreationContextFactory
    {
        internal static IObjectCreationContext CreateObjectCreationContext(Assembly assembly)
        {
            return new AutoFixtureObjectCreationContext(assembly);
        }

        internal static IParameterInfoCreation CreateParameterInfoCreation(Assembly assembly)
        {
            return new AutoFixtureObjectCreationContext(assembly);
        }
    }
}
