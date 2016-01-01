using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    public class AbstractConstrainedInputObject
    {
        public AbstractConstrainedInputObject(string value)
        {
            value.VerifyArgumentNotNullOrEmpty("Value is required")
                .VerifyArgumentLength(3, 3, true, "Value must have length of 3");

            this.Value = value;
        }

        public string Value { get; private set; }
    }
}
