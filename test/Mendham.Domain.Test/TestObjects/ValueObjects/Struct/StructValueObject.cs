using Mendham.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.ValueObjects.Struct
{
    public struct StructValueObject : IValueObject<StructValueObject>, ITestObjectDefaultProperties
    {
        public string StrVal { get; private set; }
        public int IntVal { get; private set; }

        public StructValueObject(string strVal, int intVal)
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

        public bool Equals(StructValueObject other)
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

        public static bool operator ==(StructValueObject a, StructValueObject b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(StructValueObject a, StructValueObject b)
        {
            return !(a == b);
        }
    }
}
