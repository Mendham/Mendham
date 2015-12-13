using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects
{
    public struct TestingIdentity : IHasEqualityComponents, ITestObjectDefaultProperties
    {
        public string StrVal { get; private set; }
        public int IntVal { get; private set; }

        public TestingIdentity(string strVal, int intVal)
        {
            this.StrVal = strVal;
            this.IntVal = intVal;
        }

        public IEnumerable<object> EqualityComponents
        {
            get
            {
                yield return IntVal;
                yield return StrVal;
            }
        }
    }
}
