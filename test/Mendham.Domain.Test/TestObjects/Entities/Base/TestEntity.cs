using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities.Base
{
    public class TestEntity : Entity<TestEntity>, ITestObjectDefaultProperties
    {
        public string StrVal { get; private set; }
        public int IntVal { get; private set; }
        public Guid NonIdentityValue { get; set; }

        public TestEntity(string strVal, int intVal)
        {
            this.StrVal = strVal;
            this.IntVal = intVal;
            NonIdentityValue = Guid.NewGuid();
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
