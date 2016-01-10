using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Extensions
{
    internal class EntityComponents : IHasEqualityComponents
    {
        private readonly IEnumerable<object> components;

        internal EntityComponents(IEnumerable<object> components)
        {
            components.VerifyArgumentNotNullOrEmpty(nameof(components), "Components for entity are not defined.");

            this.components = components;
        }

        public IEnumerable<object> EqualityComponents
        {
            get
            {
                return components;
            }
        }
    }
}
