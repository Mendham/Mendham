using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain
{
    public abstract class SingleFieldValueObject<T> : ValueObject
    {
        public T Value { get; protected set; }

        public SingleFieldValueObject(T value)
        {
            value.VerifyArgumentNotDefaultValue("Value is required");
            this.Value = value;
        }

        public static implicit operator T(SingleFieldValueObject<T> singleFieldValueObject)
        {
            return singleFieldValueObject.Value;
        }

        protected override IEnumerable<object> EqualityComponents
        {
            get
            {
                yield return this.Value;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}