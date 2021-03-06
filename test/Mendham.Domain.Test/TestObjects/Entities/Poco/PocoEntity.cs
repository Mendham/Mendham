﻿using Mendham.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Domain.Test.TestObjects.Entities.Poco
{
    public class PocoEntity : IEntity, ITestObjectDefaultProperties
    {
        public string StrVal { get; private set; }
        public int IntVal { get; private set; }
        public Guid NonIdentityValue { get; set; }

        public PocoEntity(string strVal, int intVal)
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

        public virtual IEnumerable<object> IdentityComponents
        {
            get
            {
                yield return StrVal;
                yield return IntVal;
            }
        }
    }
}
