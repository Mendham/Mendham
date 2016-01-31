using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Moq
{
    public static class Items
    {
        public static TEnumerable OrderAgnosticMatch<TEnumerable, TValue>(TEnumerable items)
            where TEnumerable : IEnumerable<TValue>
            where TValue : IEquatable<TValue>
        {
            var orderAgnosticComparer = OrderAgnosticComparer<TValue>.Default;
            return Match.Create<TEnumerable>(a => orderAgnosticComparer.Equals(a, items));
        }

        public static IEnumerable<TValue> OrderAgnosticMatch<TValue>(IEnumerable<TValue> items)
            where TValue : IEquatable<TValue>
        {
            return OrderAgnosticMatch<IEnumerable<TValue>, TValue>(items);
        }
    }
}
