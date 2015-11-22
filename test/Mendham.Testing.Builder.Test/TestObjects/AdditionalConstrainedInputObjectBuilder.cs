using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    public class AdditionalConstrainedInputObjectBuilder : IBuilder<ConstrainedInputObject>
    {
        public ConstrainedInputObject Build()
        {
            throw new NotImplementedException("Not needed for the test");
        }
    }
}
