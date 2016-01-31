using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq.Test.TestObjects
{
    public interface IMethodWithEnumerableParameter
    {
        void MethodWithEnumerableParameter(IEnumerable<int> items);
        void MethodWithListParameter(List<int> items);
        void MethodWithArrayParameter(int[] items);
    }
}
