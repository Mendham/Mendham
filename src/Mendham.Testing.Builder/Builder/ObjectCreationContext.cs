using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public class ObjectCreationContext : IObjectCreationContext
    {
        private readonly Assembly callerAssembly;
        private readonly IUnregisteredObjectCreationService anonymousBuilder;

        public ObjectCreationContext(Assembly callerAssembly)
        {
            this.callerAssembly = callerAssembly;
            this.anonymousBuilder = new AutoFixtureObjectCreationService();
        }

        public T Create<T>()
        {
            throw new NotImplementedException();
        }

        public T Create<T>(T seed)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> CreateMany<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> CreateMany<T>(T seed)
        {
            throw new NotImplementedException();
        }
    }
}
