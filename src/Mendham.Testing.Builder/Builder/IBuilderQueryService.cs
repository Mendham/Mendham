using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public interface IBuilderQueryService
    {
        IEnumerable<Type> GetBuilderTypes(Assembly callingAssembly);
    }
}
