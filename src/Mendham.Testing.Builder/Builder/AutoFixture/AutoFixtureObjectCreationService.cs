using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.AutoFixture
{
    public class AutoFixtureObjectCreationService : IObjectCreationContext
    {
        private readonly Fixture _fixture;

        private readonly static BuilderRegistrationManager builderRegistrationManager =
            new BuilderRegistrationManager();

        public AutoFixtureObjectCreationService(Assembly callerAssembly)
        {
            var builderRegistration = builderRegistrationManager.GetBuilderRegistration(callerAssembly);

            _fixture = new Fixture();
            _fixture.Customizations.Add(new CreateWithCountBuilder());
            _fixture.Customizations.Add(new BuilderRegistrationSpecimenBuilder(builderRegistration));
        }

        public T Create<T>()
        {
            return _fixture.Create<T>();
        }

        public T Create<T>(T seed)
        {
            return _fixture.Create(seed);
        }

        public object Create(ParameterInfo parameterInfo)
        {
            var context = new SpecimenContext(_fixture);
            return context.Resolve(parameterInfo);
        }

        public object Create(ParameterInfo parameterInfo, int countForMultiple)
        {
            var context = new CreateWithCountSpecimenContext(_fixture, countForMultiple);
            return context.Resolve(parameterInfo);
        }

        public IEnumerable<T> CreateMany<T>()
        {
            return _fixture.CreateMany<T>();
        }

        public IEnumerable<T> CreateMany<T>(T seed)
        {
            return _fixture.CreateMany(seed);
        }
    }
}
