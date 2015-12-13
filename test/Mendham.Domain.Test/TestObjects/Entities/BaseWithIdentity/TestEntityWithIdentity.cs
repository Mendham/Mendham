using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities.BaseWithIdentity
{
    public class TestEntityWithIdentity : Entity<TestEntityWithIdentity, TestingIdentity>
    {
        public Guid NonIdentityValue { get; set; }

        public TestEntityWithIdentity(TestingIdentity id) : base(id)
        {
            NonIdentityValue = Guid.NewGuid();
        }
    }
}
