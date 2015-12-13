using Mendham.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities.PocoWithIdentity
{
    public class PocoWithIdentityEntity : IEntity<TestingIdentity>
    {
        private readonly TestingIdentity _id;
        public Guid NonIdentityValue { get; set; }

        public PocoWithIdentityEntity(TestingIdentity id) 
        {
            _id = id;
            NonIdentityValue = Guid.NewGuid();
        }

        public TestingIdentity Id { get { return _id; } }

        public IEnumerable<object> IdentityComponents
        {
            get
            {
                return this.GetIdentityComponents();
            }
        }
    }
}
