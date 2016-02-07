using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain
{
    public abstract class SingleFieldValueObject<TSingleField, TValueObject> : ValueObject<TValueObject>, ISingleFieldValueObject<TSingleField>
        where TValueObject : SingleFieldValueObject<TSingleField, TValueObject>
    {
        public TSingleField Value { get; protected set; }

        public SingleFieldValueObject(TSingleField value)
        {
            value.VerifyArgumentNotDefaultValue(nameof(value), "Value is required");
            this.Value = value;
        }

        public static implicit operator TSingleField(SingleFieldValueObject<TSingleField, TValueObject> singleFieldValueObject)
        {
            return singleFieldValueObject.Value;
        }
    }
}