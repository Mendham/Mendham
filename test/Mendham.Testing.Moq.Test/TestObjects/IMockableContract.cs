using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq.Test.TestObjects
{
    public interface IMockableContract
    {
        int GetValue();
        Task<int> GetValueAsync();

        IEnumerable<int> GetValues();
        Task<IEnumerable<int>> GetValuesAsync();

        Task DoSomethingAsync();
    }
}
