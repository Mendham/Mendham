using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Other
{
    public struct PlainStructWithComponents : IHasEqualityComponents
    {
        public string StrVal { get; private set; }
        public int IntVal { get; private set; }

        public PlainStructWithComponents(string strVal, int intVal)
        {
            this.StrVal = strVal;
            this.IntVal = intVal;
        }

        public IEnumerable<object> EqualityComponents
        {
            get
            {
                yield return StrVal;
                yield return IntVal;
            }
        }
    }
}
