using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Equality
{
    public static class Components
    {
        /// <summary>
        /// Use this to define a case insenitive equality component
        /// </summary>
        /// <param name="str">String component where case does not matter</param>
        public static ComponentWithComparer<string> CaseInsensitiveComponent(string str)
        {
            return new ComponentWithComparer<string>(str, StringComparer.OrdinalIgnoreCase);
        }
    }
}
