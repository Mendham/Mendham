using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities
{
    public class DerivedTestEntity : TestEntity
    {
        public Guid derivedNonIdentityValue { get; set; }

        public DerivedTestEntity(string strVal, int intVal) : base(strVal, intVal)
        {
            derivedNonIdentityValue = Guid.NewGuid();
        }
    }
}
