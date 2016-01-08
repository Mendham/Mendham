using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple =false)]
    public class IgnoreFixtureComponentAttribute : Attribute
    {
    }
}
