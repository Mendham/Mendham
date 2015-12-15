using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Equality
{
    public class ComponentWithComparer<T> : IComponentWithComparer
    {
        private readonly T _component;
        private readonly IEqualityComparer<T> _comparer;

        public ComponentWithComparer(T component, IEqualityComparer<T> comparer)
        {
            this._component = component;
            this._comparer = comparer
                .VerifyArgumentNotDefaultValue("Comparer is required");
        }

        public T Component { get { return _component; } }
        public IEqualityComparer<T> Comparer { get { return _comparer; } }

        public bool IsEqualToComponent(object other)
        {
            if (ReferenceEquals(this, other))
                return true;

            var otherComponentWithComparer = other as ComponentWithComparer<T>;

            if (otherComponentWithComparer != default(ComponentWithComparer<T>))
            {
                return AreComponentsEqual(otherComponentWithComparer);
                     
            }

            return false;
        }

        private bool AreComponentsEqual(ComponentWithComparer<T> otherComponentWithComparer)
        {
            if (!_comparer.Equals(_component, otherComponentWithComparer.Component))
                return false;

            // Verify the comparers are the same (usually the case with static built in) or if not
            // make sure that the other comparer says they are true.
            // This is done to ensure consistant results when comparing hash codes.
            return ReferenceEquals(_comparer, otherComponentWithComparer.Comparer) ||
                otherComponentWithComparer.Comparer.Equals(_component, otherComponentWithComparer.Component);
        }

        public int GetComponentHashCode()
        {
            return _comparer.GetHashCode(_component);
        }
    }
}
