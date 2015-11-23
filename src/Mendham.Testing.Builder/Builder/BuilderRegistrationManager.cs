using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public class BuilderRegistrationManager
    {
        private readonly ConcurrentDictionary<AssemblyName, IBuilderRegistration> assemblyRegistrations;
        private readonly IBuilderRegistrationFactory builderRegistrationFactory;

        public BuilderRegistrationManager()
            : this(new BuilderRegistrationFactory())
        { }

        public BuilderRegistrationManager(IBuilderRegistrationFactory builderRegistrationFactory)
        {
            builderRegistrationFactory.VerifyArgumentNotDefaultValue("IBuilderRegistrationFactory is required");

            this.builderRegistrationFactory = builderRegistrationFactory;
            this.assemblyRegistrations = new ConcurrentDictionary<AssemblyName, IBuilderRegistration>();
        }

        public IBuilderRegistration GetBuilderRegistration(Assembly callingAssembly)
        {
            return assemblyRegistrations
                .GetOrAdd(callingAssembly.GetName(), a => CreateBuilderRegistration(callingAssembly));
        }

        private IBuilderRegistration CreateBuilderRegistration(Assembly callingAssembly)
        {
            var builderRegistration = builderRegistrationFactory.Create();
            builderRegistration.Register(callingAssembly);

            return builderRegistration;
        }
    }
}
