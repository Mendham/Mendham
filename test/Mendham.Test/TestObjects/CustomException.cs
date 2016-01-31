using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Test.TestObjects
{
    public class CustomException : Exception
    {
        public CustomException(string message)
            :base(message)
        { }
    }
}
