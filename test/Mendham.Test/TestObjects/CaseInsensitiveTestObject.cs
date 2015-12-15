using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Test.TestObjects
{
    public class CaseInsensitiveTestObject : IHasEqualityComponents
    {
        public CaseInsensitiveTestObject(string strVal)
        {
            this.StrVal = strVal;
        }

        public string StrVal { get; set; }


        public IEnumerable<object> EqualityComponents
        {
            get
            {
                yield return Components.CaseInsensitiveComponent(StrVal);
            }
        }
    }
}
