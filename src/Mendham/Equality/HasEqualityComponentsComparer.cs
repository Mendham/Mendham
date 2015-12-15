using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Equality
{
    public class HasEqualityComponentsComparer : IEqualityComparer<object>
    {
        private HasEqualityComponentsComparer()
        { }

        private const int DEFAULT_HASHCODE_FOR_NULL = -7919;

        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
                return true;

            var xComponentWithComparer = x as IComponentWithComparer;

            if (xComponentWithComparer != default(IComponentWithComparer))
            {
                return xComponentWithComparer.IsEqualToComponent(y);
            }

            var xHasEqualityComponents = x as IHasEqualityComponents;

            if (xHasEqualityComponents != default(IHasEqualityComponents))
            {
                return xHasEqualityComponents.AreComponentsEqual(y) && xHasEqualityComponents.IsObjectSameType(y);
            }
            else
            {
                return object.Equals(x, y);
            }
        }

        public int GetHashCode(object obj)
        {
            var objComponentWithComparer = obj as IComponentWithComparer;

            if (objComponentWithComparer != default(IComponentWithComparer))
            {
                return objComponentWithComparer.GetComponentHashCode();
            }

            var hasEqualityComponents = obj as IHasEqualityComponents;

            if (hasEqualityComponents != default(IHasEqualityComponents))
            {
                return hasEqualityComponents.GetObjectWithEqualityComponentsHashCode();
            }
            else if (obj != null)
            {
                return obj.GetHashCode();
            }
            else
            {
                // This just handles a null so it is not throwing a null reference and so it is counted 
                // (in opposed to it being 0 which is a much more common value from a primitive)
                return DEFAULT_HASHCODE_FOR_NULL;
            }
        }

        private static HasEqualityComponentsComparer _comparer = new HasEqualityComponentsComparer();

        public static IEqualityComparer<object> Default
        {
            get
            {
                return _comparer;
            }
        }
    }
}
