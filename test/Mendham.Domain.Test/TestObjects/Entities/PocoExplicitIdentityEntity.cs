using Mendham.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities
{
    public class PocoExplicitIdentityEntity : IEntity, ITestObjectDefaultProperties
    {
        public string StrVal { get; private set; }
        public int IntVal { get; private set; }
        public Guid NonIdentityValue { get; set; }

        public PocoExplicitIdentityEntity(string strVal, int intVal)
        {
            this.StrVal = strVal;
            this.IntVal = intVal;
            NonIdentityValue = Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            return this.IsEqualToEntity(obj as IEntity);
        }

        public override int GetHashCode()
        {
            return this.GetEntityHashCode();
        }

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
