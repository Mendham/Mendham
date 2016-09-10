using System.Net;

namespace Mendham.Testing.Http.Assertions
{
    internal static class FormattingExtensions
    {
        internal static string FormattedStatusCode(this HttpStatusCode statusCode)
        {
            return $"{statusCode.ToString()} ({(int)statusCode})";
        }
    }
}
