using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Equality
{
    public static class EqualityObjectExtensions
    {
        /// <summary>
        /// Deterines if an object is the same as the object implementing IHasEqualityComponents
        /// </summary>
        public static bool IsObjectSameType(this object x, object otherObject)
        {
            if (x == null)
                throw new NullReferenceException("Object being checked by HaveEqualComponents cannot be null");

            if (ReferenceEquals(x, otherObject))
                return true;

            if (otherObject == null)
                return false;

            return x.GetType().Equals(otherObject.GetType());
        }
    }
}
