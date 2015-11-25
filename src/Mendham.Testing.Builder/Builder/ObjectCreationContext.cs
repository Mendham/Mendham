using Mendham.Testing.Builder.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public class ObjectCreationContext : IObjectCreationContext
    {
        private readonly IBuilderRegistration builderRegistration;
        private readonly IUnregisteredObjectCreationService anonymousBuilder;

        private readonly static BuilderRegistrationManager builderRegistrationManager =
            new BuilderRegistrationManager();

        public ObjectCreationContext(Assembly callerAssembly)
        {
            this.builderRegistration = builderRegistrationManager.GetBuilderRegistration(callerAssembly);
            this.anonymousBuilder = new AutoFixtureObjectCreationService(builderRegistration);
        }

        public T Create<T>()
        {
            if (builderRegistration.IsTypeRegistered<T>())
            {
                return builderRegistration.Build<T>();
            }
            else
            {
                return anonymousBuilder.Create<T>();
            }
        }

        public T Create<T>(T seed)
        {
            if (builderRegistration.IsTypeRegistered<T>())
            {
                return builderRegistration.Build<T>();
            }
            else
            {
                return anonymousBuilder.Create<T>(seed);
            }
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
