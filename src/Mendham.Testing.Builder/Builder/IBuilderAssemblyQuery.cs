﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.DataTheoryBuilder
{
    public interface IBuilderAssemblyQuery
    {
        IEnumerable<Assembly> GetAssembliesWithBuilders();
    }
}
