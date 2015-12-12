using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities
{
    public class DerivedPocoEntity : PocoEntity
    {
        public Guid DerivedNonIdentityValue { get; set; }

        public DerivedPocoEntity(string strVal, int intVal) : base(strVal, intVal)
        {
            DerivedNonIdentityValue = Guid.NewGuid();
        }
    }
}
