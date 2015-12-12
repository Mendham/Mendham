using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities
{
    public class DerivedOverridenIdentityPocoEntity : PocoEntity
    {
        public Guid DerivedNonIdentityValue { get; set; }

        public DerivedOverridenIdentityPocoEntity(string strVal, int intVal)
            : base(strVal, intVal)
        {
            this.DerivedNonIdentityValue = Guid.NewGuid();
        }

        public override IEnumerable<object> IdentityComponents
        {
            get
            {
                yield return StrVal;
                yield return IntVal;
            }
        }
    }
}
