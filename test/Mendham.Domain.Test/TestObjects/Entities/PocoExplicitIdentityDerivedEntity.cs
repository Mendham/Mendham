using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities
{
    public class PocoExplicitIdentityDerivedEntity : PocoExplicitIdentityEntity
    {
        public Guid DerivedNonIdentityValue { get; set; }

        public PocoExplicitIdentityDerivedEntity(string strVal, int intVal) : base(strVal, intVal)
        {
            DerivedNonIdentityValue = Guid.NewGuid();
        }
    }
}
