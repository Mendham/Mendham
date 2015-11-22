using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    [MendhamBuilder]
    [MendhamBuilder(typeof(AbstractConstrainedInputObject))]
    public class ConstrainedInputObjectBuilder : DataBuilder<ConstrainedInputObject>
    {
        private string value;

        public ConstrainedInputObjectBuilder()
        {
            this.value = Guid.NewGuid()
                .ToString()
                .Substring(0, 3);
        }

        public ConstrainedInputObjectBuilder WithValue(string value)
        {
            this.value = value;
            return this;
        }

        protected override ConstrainedInputObject BuildObject()
        {
            return new ConstrainedInputObject(value);
        }
    }
}
