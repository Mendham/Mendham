using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities.Poco
{
    public class PocoDerivedEntity : PocoEntity
    {
        public Guid DerivedNonIdentityValue { get; set; }

        public PocoDerivedEntity(string strVal, int intVal) : base(strVal, intVal)
        {
            DerivedNonIdentityValue = Guid.NewGuid();
        }
    }
}
