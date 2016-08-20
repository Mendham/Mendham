using System.Data;

#if NETSTANDARD1_3
using IDbConnection = global::System.Data.Common.DbConnection;
#endif

namespace Mendham.Infrastructure.RelationalDatabase
{
    public interface IConnectionFactory
    {
        /// <summary>
        /// Generates a new IDbConnection
        /// </summary>
        /// <returns></returns>
        IDbConnection GetConnection();
    }
}
