using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Test.TestObjects
{
    public class AltTestObject : IHasEqualityComponents
    {
        public AltTestObject(string strVal, int intVal, object objVal)
        {
            this.StrVal = strVal;
            this.IntVal = intVal;
            this.ObjVal = objVal;
        }

        public string StrVal { get; set; }
        public int IntVal { get; set; }
        public object ObjVal { get; set; }


        public IEnumerable<object> EqualityComponents
        {
            get
            {
                yield return StrVal;
                yield return IntVal;
                yield return ObjVal;
            }
        }
    }
}
