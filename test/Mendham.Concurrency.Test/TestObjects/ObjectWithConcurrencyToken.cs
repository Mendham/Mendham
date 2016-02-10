using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency.Test.TestObjects
{
    public class ObjectWithConcurrencyToken : IHasConcurrencyToken
    {
        IConcurrencyToken IHasConcurrencyToken.Token { get; set; }

        public static ObjectWithConcurrencyToken WithoutToken()
        {
            return new ObjectWithConcurrencyToken();
        }

        public static ObjectWithConcurrencyToken WithToken()
        {
            IHasConcurrencyToken objWithConcurrencyToken = new ObjectWithConcurrencyToken();
            objWithConcurrencyToken.Token = new AltConcurrencyToken();

            return objWithConcurrencyToken as ObjectWithConcurrencyToken;
        }

        public static ObjectWithConcurrencyToken WithToken(IConcurrencyToken token)
        {
            token.VerifyArgumentNotNull(nameof(token));

            IHasConcurrencyToken objWithConcurrencyToken = new ObjectWithConcurrencyToken();
            objWithConcurrencyToken.Token = token;

            return objWithConcurrencyToken as ObjectWithConcurrencyToken;
        }

        public static ObjectWithConcurrencyToken WithTokenBytes(byte[] tokenBytes)
        {
            tokenBytes.VerifyArgumentNotNullOrEmpty(nameof(tokenBytes));

            IHasConcurrencyToken objWithConcurrencyToken = new ObjectWithConcurrencyToken();
            objWithConcurrencyToken.Token = new Int64ConcurrencyToken(tokenBytes);

            return objWithConcurrencyToken as ObjectWithConcurrencyToken;
        }

        public IConcurrencyToken GetToken()
        {
            return (this as IHasConcurrencyToken).Token;
        }
    }
}
