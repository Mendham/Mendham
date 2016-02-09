using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.ValueObjects.CustomEquality
{
    public class AltCustomEqualityComponentsValueObject : ValueObject<AltCustomEqualityComponentsValueObject>
    {
        public string StrVal { get; private set; }
        public int IntVal { get; private set; }

        public AltCustomEqualityComponentsValueObject(string strVal, int intVal)
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
