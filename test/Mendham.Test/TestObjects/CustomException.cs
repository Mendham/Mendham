using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Test.TestObjects
{
    public class CustomException : Exception
    {
        public object Value { get; private set; }

        public CustomException(string message, object value)
            : base(message)
        {
            this.Value = value;
        }
    }
}
