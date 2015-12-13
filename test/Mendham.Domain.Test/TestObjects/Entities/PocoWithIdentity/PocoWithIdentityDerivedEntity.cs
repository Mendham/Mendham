using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities.PocoWithIdentity
{
    public class PocoWithIdentityDerivedEntity : PocoWithIdentityEntity
    {
        public Guid DerivedNonIdentityValue { get; set; }

        public PocoWithIdentityDerivedEntity(TestingIdentity id) : base(id)
        {
            DerivedNonIdentityValue = Guid.NewGuid();
        }
    }     
}
