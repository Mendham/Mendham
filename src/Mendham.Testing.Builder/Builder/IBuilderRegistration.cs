using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public interface IBuilderRegistration
    {
        bool IsTypeRegistered<T>();
        bool IsTypeRegistered(Type typeToBuild);

        T Build<T>();
        object Build(Type typeToBuild);
    }
}
