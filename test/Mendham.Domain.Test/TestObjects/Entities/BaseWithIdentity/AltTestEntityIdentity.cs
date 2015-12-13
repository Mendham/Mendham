using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities.BaseWithIdentity
{
    public class AltTestEntityIdentity : Entity<AltTestEntityIdentity, TestingIdentity>
    {
        public Guid nonIdentityValue { get; set; }

        public AltTestEntityIdentity(TestingIdentity id) : base(id)
        {
            nonIdentityValue = Guid.NewGuid();
        }
    }
}
