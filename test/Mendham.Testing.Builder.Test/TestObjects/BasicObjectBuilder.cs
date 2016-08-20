using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    public class BasicObjectBuilder : Builder<BasicObject>
    {
        int _intVal;
        string _stringVal;

        public BasicObjectBuilder()
        {
            _intVal = ObjectCreationContext.Create<int>();
            _stringVal = ObjectCreationContext.Create("str");
        }

        public BasicObjectBuilder WithIntVal(int intVal)
        {
            _intVal = intVal;
            return this;
        }

        public BasicObjectBuilder WithStringVal(string stringVal)
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
