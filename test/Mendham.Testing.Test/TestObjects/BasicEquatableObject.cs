using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Test.TestObjects
{
    public class BasicEquatableObject : IHasEqualityComponents, IEquatable<BasicEquatableObject>
    {
        public int IntVal { get; private set; }
        public string StringVal { get; private set; }

        public BasicEquatableObject(int intVal, string stringVal)
        {
            intVal.VerifyArgumentNotDefaultValue(nameof(intVal));
            stringVal.VerifyArgumentNotNullOrWhiteSpace(nameof(stringVal));

            this.IntVal = intVal;
            this.StringVal = stringVal;
        }

        public IEnumerable<object> EqualityComponents
        {
            get
            {
                yield return IntVal;
                yield return StringVal;
            }
        }

        public override bool Equals(object obj)
        {
            return HasEqualityComponentsComparer.Default.Equals(this, obj);
        }

        public override int GetHashCode()
        {
            return HasEqualityComponentsComparer.Default.GetHashCode(this);
        }

        public bool Equals(BasicEquatableObject other)
        {
            return HasEqualityComponentsComparer.Default.Equals(this, other);
        }
    }
}
