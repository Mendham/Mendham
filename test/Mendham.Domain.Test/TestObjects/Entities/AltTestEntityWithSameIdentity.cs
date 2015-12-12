using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities
{
    public class AltTestEntityWithSameIdentity : Entity<AltTestEntityWithSameIdentity>, ITestObjectDefaultProperties
    {
        public string StrVal { get; private set; }
        public int IntVal { get; private set; }
        public Guid nonIdentityValue { get; set; }

        public AltTestEntityWithSameIdentity(string strVal, int intVal)
        {
            this.StrVal = strVal;
            this.IntVal = intVal;
            nonIdentityValue = Guid.NewGuid();
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
