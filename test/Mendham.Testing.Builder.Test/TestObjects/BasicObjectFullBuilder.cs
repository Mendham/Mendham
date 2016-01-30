using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    public class BasicObjectFullBuilder : Builder<BasicObject, BasicObjectBuilder>
    {
        int intVal;
        string stringVal;

        public BasicObjectFullBuilder()
        {
            this.intVal = ObjectCreationContext.Create<int>();
            this.stringVal = ObjectCreationContext.Create("str");
        }

        public BasicObjectFullBuilder WithIntVal(int intVal)
        {
            this.intVal = intVal;
            return this;
        }

        public BasicObjectFullBuilder WithStringVal(string stringVal)
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
