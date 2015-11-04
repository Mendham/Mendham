using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    public interface IMockableObjectBuilder<T> : IBuilder<T>
        where T : class
    {
        T BuildAsMock();
    }
}
