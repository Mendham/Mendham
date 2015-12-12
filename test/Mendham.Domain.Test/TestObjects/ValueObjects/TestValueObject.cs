using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.ValueObjects
{
    public class TestValueObject : ValueObject<TestValueObject>, ITestObjectDefaultProperties
    {
        public string StrVal { get; private set; }
        public int IntVal { get; private set; }

        public TestValueObject(string strVal, int intVal)
        {
            this.StrVal = strVal;
            this.IntVal = intVal;
        }
    }
}
