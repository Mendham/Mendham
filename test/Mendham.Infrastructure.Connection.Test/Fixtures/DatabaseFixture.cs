using Mendham.Infrastructure.Connection.Test.Helpers;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if DNXCORE50
using IDbConnection = global::System.Data.Common.DbConnection;
#endif

namespace Mendham.Infrastructure.Connection.Test.Fixtures
{
    public class DatabaseFixture : Fixture<Func<IDbConnection>>, IDisposable
    {
        private const string INITIAL_CATALOG = "master";
        private const string TEST_DATABASE = "MendhamConnectionTest";

        public DatabaseFixture()
        {
            CreateDatabase();
            CreateAndSeedTables();
        }

        public override Func<IDbConnection> CreateSut()
        {
            return () => GetConnection(TEST_DATABASE);
        }

        public override void ResetFixture()
        {
            base.ResetFixture();
        }


        public void Dispose()
        {
            DropDatabase();
        }

        public IEnumerable<int> KnownInts
        {
            get
            {
                return Enumerable.Range(1, 1000)
                    .Select(a => a * 2);
            }
        }

        public IEnumerable<Guid> KnownGuids
        {
            get
            {
                yield return new Guid("{2333F59D-C9B0-4E85-B851-D8A2DB67725E}");
                yield return new Guid("{4FF4427F-0D57-4DBD-A2FE-31B40E04F3AB}");
                yield return new Guid("{B726DF11-4ACA-4C5F-9E2E-16E715715234}");
                yield return new Guid("{B726DF11-4ACA-4C5F-9E2E-16E715715234}");
                yield return new Guid("{8E87D4E5-22CB-4DCA-8563-7CCA1FA72AEE}");
                yield return new Guid("{1320CCC3-2CA0-49EE-B6B8-6C6B75493EF0}");
            }
        }

        public IEnumerable<string> KnownStrings
        {
            get
            {
                return KnownGuids.Select(a => a.ToString());
            }
        }

        public IEnumerable<CompositeId> KnownCompositeIds
        {
            get
            {
                var guids = KnownGuids.ToList();
                for (int i = 0; i < guids.Count(); i++)
                {
                    yield return new CompositeId
                    {
                        GuidVal = guids[i],
                        IntVal = i
                    };
                }
            }
        }

        public CompositeIdMapping GetCompositeIdMapping()
        {
            return new CompositeIdMapping();
        }

        private const string DEFAULT_CONNECITON_STRING =
            @"Data Source=(localdb)\MendhamIntegrationTest;Initial Catalog={0};Persist Security Info=False;Integrated Security=true;MultipleActiveResultSets=True";

        private SqlConnection GetConnection(string databaseName)
        {
            var connFormatStr = DEFAULT_CONNECITON_STRING;

            // Check Environment for alternative
            var connFromEnv = Environment.GetEnvironmentVariable("MENDHAM_INTEGRATION_DATABASE");
            if (!string.IsNullOrWhiteSpace(connFromEnv))
                connFormatStr = connFromEnv;

            var connStr =  string.Format(connFormatStr, databaseName);

            return new SqlConnection(connStr);
        }

        private void CreateDatabase()
        {
            DropDatabase();

            var cmdText = string.Format(@"
                CREATE DATABASE {0};
                ", TEST_DATABASE);

            using (var conn = GetConnection(INITIAL_CATALOG))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void DropDatabase()
        {
            var cmdText = string.Format(@"
                IF EXISTS(select * from sys.databases where name='{0}')
                BEGIN
                    DECLARE @kill varchar(8000) = '';
                    SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), spid) + ';'
                    FROM master..sysprocesses 
                    WHERE dbid = db_id('{0}')

                    EXEC(@kill);

                    DROP DATABASE {0};
                END
                ", TEST_DATABASE);

            using (var conn = GetConnection(INITIAL_CATALOG))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void CreateAndSeedTables()
        {
            StringBuilder cmdTestSb = new StringBuilder();
            cmdTestSb.AppendLine("CREATE TABLE IntTable (Id INT PRIMARY KEY)");
            cmdTestSb.AppendLine("CREATE TABLE StrTable (Id VARCHAR(100) PRIMARY KEY)");
            cmdTestSb.AppendLine("CREATE TABLE GuidTable (Id UNIQUEIDENTIFIER PRIMARY KEY)");

            cmdTestSb.AppendLine(@"CREATE TABLE CompositeIdTable 
                (
                    GuidVal UNIQUEIDENTIFIER NOT NULL, 
                    IntVal INT NOT NULL,
                    CONSTRAINT [PK_CompositeIdTable] PRIMARY KEY (GuidVal, IntVal)
                )");

            foreach (var num in Enumerable.Range(1, 2000))
                cmdTestSb.AppendFormat("INSERT INTO IntTable (Id) VALUES ({0}) \n", num);

            var guids = KnownGuids
                .Union(Enumerable.Range(1, 100).Select(a => Guid.NewGuid()))
                .OrderBy(a => Guid.NewGuid());

            foreach (var guid in guids)
                cmdTestSb.AppendFormat("INSERT INTO GuidTable (Id) VALUES ('{0}') \n", guid);

            foreach (var guidStr in guids.Select(a => a.ToString()))
                cmdTestSb.AppendFormat("INSERT INTO StrTable (Id) VALUES ('{0}') \n", guidStr);

            var compositeIds = KnownCompositeIds
                .Union(Enumerable.Range(100, 100)
                    .Select(a => new CompositeId { GuidVal = Guid.NewGuid(), IntVal = a }))
                .OrderBy(a => Guid.NewGuid());

            foreach (var val in compositeIds)
                cmdTestSb.AppendFormat("INSERT INTO CompositeIdTable (GuidVal, IntVal) VALUES ('{0}', {1}) \n", 
                    val.GuidVal, val.IntVal);

            using (var conn = GetConnection(TEST_DATABASE))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(cmdTestSb.ToString(), conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}