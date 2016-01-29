using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    /// <summary>
    /// Builder allows for object being built to be mockable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMockableObjectBuilder<T> : IBuilder<T>
    {
        T BuildAsMock();
    }
}
