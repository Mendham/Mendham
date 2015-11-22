using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    internal static class BuilderExtensions
    {
        internal static bool IsIBuilderInterface(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IBuilder<>);
        }

        internal static bool ImplementsIBuilder(this Type type)
        {
            return type.GetInterfaces()
                .Any(a => a.IsIBuilderInterface());
        }

        internal static Type GetIBuilderInterface(this Type type)
        {
            var result = type
                .GetInterfaces()
                .FirstOrDefault(IsIBuilderInterface);

            if (type == default(Type))
            {
                var msg = string.Format(
                    CultureInfo.CurrentCulture,
                    "Type {0} does not implement IBuilder<T>",
                    type.FullName);

                throw new InvalidOperationException(msg);
            }

            return result;
        }

        internal static Type GetTypeIBuilderBuilds(this Type type)
        {
            return type
                .GetIBuilderInterface()
                .GetGenericArguments()[0];
        }
    }
}
