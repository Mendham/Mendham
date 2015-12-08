using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    public class BasicObjectBuilder : Builder<BasicObject>
    {
        int intVal;
        string stringVal;

        public BasicObjectBuilder()
        {
            this.intVal = CreateAnonymous<int>();
            this.stringVal = CreateAnonymous("str");
        }

        public BasicObjectBuilder WithIntVal(int intVal)
        {
            this.intVal = intVal;
            return this;
        }

        public BasicObjectBuilder WithStringVal(string stringVal)
        {
            this.stringVal = stringVal;
            return this;
        }

        protected override BasicObject BuildObject()
        {
            return new BasicObject(intVal, stringVal);
        }
    }
}
