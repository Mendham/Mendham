using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public interface IBuilderLookup
    {
        IEnumerable<Type> GetBuilderTypes();
    }
}
