using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.Http
{
    public class InvalidMediaTypeException : Exception
    {
        public InvalidMediaTypeException(string expectedMediaType, string actualMediaType)
        {
            ExpectedMediaType = expectedMediaType;
            ActualMediaType = actualMediaType;
        }

        public string ActualMediaType { get; }
        public string ExpectedMediaType { get; }

        public override string Message
        {
            get
            {
                return $"The operation failed because the media type was not as expected (\"{ExpectedMediaType}\"). "
                    + $"The actual media type was \"{ActualMediaType}\".";
            }
        }
    }
}
