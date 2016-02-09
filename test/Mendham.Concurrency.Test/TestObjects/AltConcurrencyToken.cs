using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency.Test.TestObjects
{
    public class AltConcurrencyToken : IConcurrencyToken
    {
        public string DisplayValue
        {
            get
            {
                return "AltConcurrencyToken display value";
            }
        }

        public bool Equals(IConcurrencyToken other)
        {
            throw new NotImplementedException("Should not be called for testing");
        }
    }
}
