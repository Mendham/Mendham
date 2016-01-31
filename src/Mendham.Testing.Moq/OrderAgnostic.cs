using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq
{
    public static class OrderAgnostic
    {
        public static TEnumerable Match<TEnumerable, TValue>(TEnumerable items)
            where TEnumerable : IEnumerable<TValue>
            where TValue : IEquatable<TValue>
        {
            var orderAgnosticComparer = OrderAgnosticComparer<TValue>.Default;
            return global::Moq.Match.Create<TEnumerable>(a => orderAgnosticComparer.Equals(a, items));
        }

        public static IEnumerable<TValue> Match<TValue>(IEnumerable<TValue> items)
            where TValue : IEquatable<TValue>
        {
            return Match<IEnumerable<TValue>, TValue>(items);
        }
    }
}
