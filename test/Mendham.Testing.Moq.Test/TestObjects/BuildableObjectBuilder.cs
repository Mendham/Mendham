using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq.Test.TestObjects
{
    public class BuildableObjectBuilder : IMockableObjectBuilder<BuildableObject>
    {
        public BuildableObject Build()
        {
            throw new NotImplementedException();
        }

        public BuildableObject BuildAsMock()
        {
            return Mock.Of<BuildableObject>();
        }
    }
}
