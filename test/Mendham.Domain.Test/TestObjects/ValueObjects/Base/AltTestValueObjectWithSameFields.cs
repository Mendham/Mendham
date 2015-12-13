using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.ValueObjects.Base
{
    public class AltTestValueObjectWithSameFields : ValueObject<AltTestValueObjectWithSameFields>, ITestObjectDefaultProperties
    {
        public string StrVal { get; private set; }
        public int IntVal { get; private set; }

        public AltTestValueObjectWithSameFields(string strVal, int intVal)
        {
            this.StrVal = strVal;
            this.IntVal = intVal;
        }
    }
}
