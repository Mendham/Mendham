using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public interface IObjectCreationContext
    {
        T Create<T>();
        T Create<T>(T seed);

        object Create(ParameterInfo parameterInfo);
        object Create(ParameterInfo parameterInfo, int countForMultiple);

        IEnumerable<T> CreateMany<T>();
        IEnumerable<T> CreateMany<T>(T seed);
    }
}
