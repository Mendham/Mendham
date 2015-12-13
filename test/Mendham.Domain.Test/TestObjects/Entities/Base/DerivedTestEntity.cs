using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities.Base
{
    public class DerivedTestEntity : TestEntity
    {
        public Guid DerivedNonIdentityValue { get; set; }

        public DerivedTestEntity(string strVal, int intVal) : base(strVal, intVal)
        {
            DerivedNonIdentityValue = Guid.NewGuid();
        }
    }
}
