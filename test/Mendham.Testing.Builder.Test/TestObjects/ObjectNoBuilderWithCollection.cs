using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    /// <summary>
    /// This class does not have a builder associated with it
    /// </summary>
    public class ObjectNoBuilderWithCollection
    {
        public ObjectNoBuilderWithCollection()
        { }

        public IEnumerable<int> Collection {get; set;}
    }
}
