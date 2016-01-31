using System.Data;

#if DOTNET5_4
using IDbConnection = global::System.Data.Common.DbConnection;
#endif

namespace Mendham.Infrastructure.DBConnection
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
