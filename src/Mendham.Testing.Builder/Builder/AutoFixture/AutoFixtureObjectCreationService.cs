using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.AutoFixture
{
    public class AutoFixtureObjectCreationService : IUnregisteredObjectCreationService
    {
        private readonly Fixture _fixture;

        public AutoFixtureObjectCreationService(IBuilderRegistration builderRegistration)
        {
            _fixture = new Fixture();
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

        public object Create(Type type)
        {
            var context = new SpecimenContext(_fixture);
            return context.Resolve(type);
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
