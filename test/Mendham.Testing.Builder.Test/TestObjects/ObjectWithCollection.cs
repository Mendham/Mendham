﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    public class ObjectWithCollection
    {
        public ObjectWithCollection()
        { }

        public IEnumerable<int> Collection { get; set; }
    }
}
