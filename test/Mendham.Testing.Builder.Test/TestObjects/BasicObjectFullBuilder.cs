using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    public class BasicObjectFullBuilder : Builder<BasicObject, BasicObjectBuilder>
    {
        int _intVal;
        string _stringVal;

        public BasicObjectFullBuilder()
        {
            _intVal = ObjectCreationContext.Create<int>();
            _stringVal = ObjectCreationContext.Create("str");
        }

        public BasicObjectFullBuilder WithIntVal(int intVal)
        {
            _intVal = intVal;
            return this;
        }

        public BasicObjectFullBuilder WithStringVal(string stringVal)
        {
            _stringVal = stringVal;
            return this;
        }

        protected override BasicObject BuildObject()
        {
            return new BasicObject(_intVal, _stringVal);
        }
    }
}
