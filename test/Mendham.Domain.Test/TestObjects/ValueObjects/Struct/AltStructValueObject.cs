using Mendham.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.ValueObjects.Struct
{
    public struct AltStructValueObject : IValueObject<AltStructValueObject>, ITestObjectDefaultProperties
    {
        public string StrVal { get; private set; }
        public int IntVal { get; private set; }

        public AltStructValueObject(string strVal, int intVal)
        {
            this.StrVal = strVal;
            this.IntVal = intVal;
        }

        public override bool Equals(object obj)
        {
            return this.IsEqualToValueObject(obj);
        }

        public override int GetHashCode()
        {
            return this.GetValueObjectHashCode();
        }

        public bool Equals(AltStructValueObject other)
        {
            return this.IsEqualToValueObject(other);
        }

        public IEnumerable<object> EqualityComponents
        {
            get
            {
                yield return StrVal;
                yield return IntVal;
            }
        }
    }
}
