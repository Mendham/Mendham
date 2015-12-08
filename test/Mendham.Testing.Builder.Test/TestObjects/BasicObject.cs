using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    public class BasicObject
    {
        public int IntVal { get; private set; }
        public string StringVal { get; private set; }

        public BasicObject (int intVal, string stringVal)
        {
            intVal.VerifyArgumentNotDefaultValue("Non default int is required");
            stringVal.VerifyArgumentNotNullOrWhiteSpace("String is required");

            this.IntVal = intVal;
            this.StringVal = stringVal;
        }
    }
}
