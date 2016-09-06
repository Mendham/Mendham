using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Http.Test.TestObjects
{
    public class BasicObject : IEquatable<BasicObject>, IHasEqualityComponents
    {
        public string Value1 { get; set; }
        public int Value2 { get; set; }

        IEnumerable<object> IHasEqualityComponents.EqualityComponents
        {
            get
            {
                yield return Value1;
                yield return Value2;
            }
        }

        public bool Equals(BasicObject other)
        {
            return this.AreComponentsEqual(other);
        }

        public override string ToString()
        {
            return $"BasicObject {{ Value1=\"{Value1}\", Value2={Value2} }}";
        }
    }

    public class BasicObjectComparer : IEqualityComparer<BasicObject>
    {
        public bool Equals(BasicObject x, BasicObject y)
        {
            return x.AreComponentsEqual(y);
        }

        public int GetHashCode(BasicObject obj)
        {
            return obj.GetObjectWithEqualityComponentsHashCode();
        }
    }
}
