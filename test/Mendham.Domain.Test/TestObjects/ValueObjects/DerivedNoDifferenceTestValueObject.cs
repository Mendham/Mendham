using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.ValueObjects
{
    public class DerivedNoDifferenceTestValueObject : TestValueObject
    {
        public DerivedNoDifferenceTestValueObject(string strVal, int intVal) : base(strVal, intVal)
        {
        }
    }
}
