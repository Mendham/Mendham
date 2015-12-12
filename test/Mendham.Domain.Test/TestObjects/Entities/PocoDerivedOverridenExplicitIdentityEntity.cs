using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities
{
    public class PocoDerivedOverridenExplicitIdentityEntity : PocoExplicitIdentityEntity, IEntity
    {
        public Guid DerivedNonIdentityValue { get; set; }

        public PocoDerivedOverridenExplicitIdentityEntity(string strVal, int intVal)
            : base(strVal, intVal)
        {
            this.DerivedNonIdentityValue = Guid.NewGuid();
        }

        // This is an "override" of the explicit IdentityComponents in base class
        IEnumerable<object> IEntity.IdentityComponents
        {
            get
            {
                yield return StrVal;
                yield return IntVal;
            }
        }
    }
}
