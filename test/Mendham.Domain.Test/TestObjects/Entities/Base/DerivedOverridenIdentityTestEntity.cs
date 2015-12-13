using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities.Base
{
    public class DerivedOverridenIdentityTestEntity : TestEntity
    {
        public Guid DerivedNonIdentityValue { get; private set; }

       public DerivedOverridenIdentityTestEntity(string strVal, int intVal)
            : base(strVal, intVal)
        {
            this.DerivedNonIdentityValue = Guid.NewGuid();
        }

        protected override IEnumerable<object> IdentityComponents
        {
            get
            {
                yield return StrVal;
                yield return IntVal;
            }
        }
    }
}
