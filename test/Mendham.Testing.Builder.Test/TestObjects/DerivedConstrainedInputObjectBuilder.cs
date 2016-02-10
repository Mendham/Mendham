using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    [DefaultBuilder]
    public class DerivedConstrainedInputObjectBuilder : Builder<DerivedConstrainedInputObject>
    {
        private string value;
        private int derivedValue;

        public DerivedConstrainedInputObjectBuilder()
        {
            this.value = ObjectCreationContext.Create<string>()
                .Substring(0, 3);
            this.derivedValue = ObjectCreationContext.Create<int>();
        }

        public DerivedConstrainedInputObjectBuilder WithValue(string value)
        {
            this.value = value;
            return this;
        }

        public DerivedConstrainedInputObjectBuilder WithDerivedValue(int derivedValue)
        {
            this.derivedValue = derivedValue;
            return this;
        }

        protected override DerivedConstrainedInputObject BuildObject()
        {
            return new DerivedConstrainedInputObject(value, derivedValue);
        }
    }
}