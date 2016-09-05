using Mendham.Equality;
using System.Collections.Generic;
using System;

namespace Mendham.Infrastructure.Http.Test.TestObjects
{
    public class SimpleObject : IHasEqualityComponents
    {
        public string Value1 { get; set; }
        public int Value2 { get; set; }


        public override bool Equals(object obj)
        {
            return this.AreComponentsEqual(obj);
        }

        public override int GetHashCode()
        {
            return this.GetObjectWithEqualityComponentsHashCode();
        }

        IEnumerable<object> IHasEqualityComponents.EqualityComponents
        {
            get
            {
                yield return Value1;
                yield return Value2;
            }
        }

        public const string FormatString = "{{\"value1\":\"{0}\",\"value2\":{1}}}";
    }
}
