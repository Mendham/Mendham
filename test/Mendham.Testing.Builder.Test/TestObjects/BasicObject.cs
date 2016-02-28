using System;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    public class BasicObject : IEquatable<BasicObject>
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

        public bool Equals(BasicObject other)
        {
            if (other == null)
            {
                throw new NotImplementedException("No handling for null types");
            }

            return IntVal.Equals(other.IntVal) && StringVal.Equals(other.StringVal);
        }
    }
}
