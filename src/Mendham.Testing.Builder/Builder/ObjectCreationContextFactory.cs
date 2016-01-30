using Mendham.Testing.Builder.AutoFixture;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public static class ObjectCreationContextFactory
    {
        public static IObjectCreationContext Create(Assembly assembly)
        {
            return new AutoFixtureObjectCreationContext(assembly);
        }
    }
}
