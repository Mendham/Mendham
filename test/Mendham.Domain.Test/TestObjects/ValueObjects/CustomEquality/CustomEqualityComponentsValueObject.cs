using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.ValueObjects.CustomEquality
{
    public class CustomEqualityComponentsValueObject : ValueObject<CustomEqualityComponentsValueObject>
    {
        public string StrVal { get; private set; }
        public int IntVal { get; private set; }

        public CustomEqualityComponentsValueObject(string strVal, int intVal)
        {
            this.StrVal = strVal;
            this.IntVal = intVal;
        }

        protected override IEnumerable<object> CustomEqualityComponents
        {
            get
            {
                yield return StrVal;
            }
        }
    }
}
