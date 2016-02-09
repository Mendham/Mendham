using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency
{
    public static class Int64ConcurrencyTokenExtensions
    {
        public static T ValidateByteConcurrencyToken<T>(this T obj, byte[] tokenBytes, string message = null)
            where T : IHasConcurrencyToken
        {
            tokenBytes.VerifyArgumentNotNullOrEmpty(nameof(tokenBytes), "Bytes for token are required");

            IConcurrencyToken token = new Int64ConcurrencyToken(tokenBytes);

            return obj.ValidateConcurrencyToken(token, message);
        }

        public static T SetByteConcurrencyToken<T>(this T obj, byte[] tokenBytes)
           where T : IHasConcurrencyToken
        {
            tokenBytes.VerifyArgumentNotNullOrEmpty(nameof(tokenBytes), "Bytes for token are required");

            IConcurrencyToken token = new Int64ConcurrencyToken(tokenBytes);

            return obj.SetConcurrencyToken(token);
        }
    }
}
