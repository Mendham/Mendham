using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities.BaseWithIdentity
{
    public class DerivedTestEntityWithIdentity : TestEntityWithIdentity
    {
        public Guid DerivedNonIdentityValue { get; set; }

        public DerivedTestEntityWithIdentity(TestingIdentity id) : base(id)
        {
            DerivedNonIdentityValue = Guid.NewGuid();
        }
    }
}
