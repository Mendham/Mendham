using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public interface IBuilderRegistration
    {
        void Register(Assembly callingAssembly);

        bool IsTypeRegistered<T>();

        T Build<T>();

        bool TryBuild<T>(out T value);
    }
}
