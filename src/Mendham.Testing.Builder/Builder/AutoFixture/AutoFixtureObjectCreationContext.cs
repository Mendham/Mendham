using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Reflection;

namespace Mendham.Testing.Builder.AutoFixture
{
    internal class AutoFixtureObjectCreationContext : IObjectCreationContext, IParameterInfoCreation
    {
        private static readonly Fixture _fixture;

        static AutoFixtureObjectCreationContext()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new WithCountBuilder());
            _fixture.Customizations.Add(new BuilderRegistrationSpecimenBuilder());
        }

        private readonly Assembly callingAssembly;

        public AutoFixtureObjectCreationContext(Assembly callingAssembly)
        {
            this.callingAssembly = callingAssembly
                .VerifyArgumentNotDefaultValue(nameof(callingAssembly));
        }

        public T Create<T>()
        {
            return _fixture
                .CreateContext(callingAssembly)
                .Create<T>();
        }

        public T Create<T>(T seed)
        {
            return _fixture
                .CreateContext(callingAssembly)
                .Create(seed);
        }

        public object Create(ParameterInfo parameterInfo)
        {
            return _fixture
                .CreateContext(callingAssembly)
                .Resolve(parameterInfo);
        }

        public object Create(ParameterInfo parameterInfo, int countForMultiple)
        {
            var context = new WithCountSpecimenContext(_fixture, callingAssembly, countForMultiple);
            return context.Resolve(parameterInfo);
        }

        public IEnumerable<T> CreateMany<T>()
        {
            return _fixture
                .CreateContext(callingAssembly)
                .CreateMany<T>();
        }

        public IEnumerable<T> CreateMany<T>(T seed)
        {
            return _fixture
                .CreateContext(callingAssembly)
                .CreateMany(seed);
        }

        public IEnumerable<T> CreateMany<T>(int count)
        {
            return _fixture
                .CreateContext(callingAssembly)
                .CreateMany<T>(count);
        }

        public IEnumerable<T> CreateMany<T>(T seed, int count)
        {
            return _fixture
                .CreateContext(callingAssembly)
                .CreateMany(seed, count);
        }
    }
}
