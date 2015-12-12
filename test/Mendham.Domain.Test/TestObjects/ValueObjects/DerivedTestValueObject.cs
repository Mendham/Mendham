using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.ValueObjects
{
    public class DerivedTestValueObject : TestValueObject
    {
        public string DerivedStrVal { get; private set; }

        public DerivedTestValueObject(string strVal, int intVal, string derivedStrVal) : base(strVal, intVal)
        {
            this.DerivedStrVal = derivedStrVal;
        }
    }
}
