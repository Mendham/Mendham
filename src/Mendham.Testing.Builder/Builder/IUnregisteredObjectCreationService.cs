using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public interface IUnregisteredObjectCreationService
    {
        T Create<T>();
        T Create<T>(T seed);
        IEnumerable<T> CreateMany<T>();
        IEnumerable<T> CreateMany<T>(T seed);
    }
}
