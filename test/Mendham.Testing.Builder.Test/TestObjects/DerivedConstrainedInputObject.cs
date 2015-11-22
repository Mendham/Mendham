using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    public class DerivedConstrainedInputObject : ConstrainedInputObject
    {
        public DerivedConstrainedInputObject(string value, int derivedValue) 
            : base(value)
        {
            derivedValue.VerifyArgumentNotDefaultValue("Derived value cannot be default");

            this.DerivedValue = derivedValue;
        }

        public int DerivedValue { get; private set; }
    }
}
