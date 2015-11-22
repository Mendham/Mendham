using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public class AutoFixtureObjectCreationService : IUnregisteredObjectCreationService
    {
        private readonly Fixture _fixture;

        public AutoFixtureObjectCreationService()
        {
            this._fixture = new Fixture();
        }

        public T Create<T>()
        {
            return _fixture.Create<T>();
        }

        public T Create<T>(T seed)
        {
            return _fixture.Create<T>(seed);
        }

        public IEnumerable<T> CreateMany<T>()
        {
            return _fixture.CreateMany<T>();
        }

        public IEnumerable<T> CreateMany<T>(T seed)
        {
            return _fixture.CreateMany<T>();
        }
    }
}
