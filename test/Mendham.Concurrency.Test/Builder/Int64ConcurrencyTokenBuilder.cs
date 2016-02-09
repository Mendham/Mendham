using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency.Test.Builder
{
    [MendhamBuilder]
    public class Int64ConcurrencyTokenBuilder : Builder<Int64ConcurrencyToken>
    {
        long value;

        public Int64ConcurrencyTokenBuilder()
        {
            value = ObjectCreationContext.Create<long>();
        }

        protected override Int64ConcurrencyToken BuildObject()
        {
            return new Int64ConcurrencyToken(value);
        }
    }
}
