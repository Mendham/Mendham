using Dapper;
using Mendham.Infrastructure.Dapper.Test.Fixtures;
using Mendham.Infrastructure.Dapper.Test.Helpers;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Infrastructure.Dapper.Test
{
    public class ConnectionWithSetTest : BaseUnitTest<DatabaseFixture>
    {
        public ConnectionWithSetTest(DatabaseFixture fixture) : base(fixture)
        {
        }


        [Fact]
        public async Task ConnectionWithSet_IntDefaultMapping_AllSelectedValues()
        {
            var mapping = DefaultConnectionWithSetMapping.Get<int>();
            using (var conn = new ConnectionWithSet<int>(TestFixture.CreateSut(), mapping))
            {
                await conn.OpenAsync(TestFixture.KnownInts);

                var q = await conn.QueryAsync<int>(@"
                    SELECT Id
                    FROM IntTable it
                        INNER JOIN #Items items ON it.Id = items.Value
                ");

                var result = q.ToList();

                Assert.NotEmpty(result);
                Assert.Equal(TestFixture.KnownInts.Count(), result.Count());
                Assert.Equal(TestFixture.KnownInts.OrderBy(a => a), result.OrderBy(a => a));
            }
        }

        [Fact]
        public async Task ConnectionWithSet_GuidDefaultMapping_AllSelectedValues()
        {
            var mapping = DefaultConnectionWithSetMapping.Get<Guid>();
            using (var conn = new ConnectionWithSet<Guid>(TestFixture.CreateSut(), mapping))
            {
                await conn.OpenAsync(TestFixture.KnownGuids);

                var q = await conn.QueryAsync<Guid>(@"
                    SELECT Id
                    FROM GuidTable gt
                        INNER JOIN #Items items ON gt.Id = items.Value
                ");

                var result = q.ToList();

                Assert.NotEmpty(result);
                Assert.Equal(TestFixture.KnownGuids.Count(), result.Count());
                Assert.Equal(TestFixture.KnownGuids.OrderBy(a => a), result.OrderBy(a => a));
            }
        }

        [Fact]
        public async Task ConnectionWithSet_StringDefaultMapping_AllSelectedValues()
        {
            var mapping = DefaultConnectionWithSetMapping.Get<string>();
            using (var conn = new ConnectionWithSet<string>(TestFixture.CreateSut(), mapping))
            {
                await conn.OpenAsync(TestFixture.KnownStrings);

                var q = await conn.QueryAsync<string>(@"
                    SELECT Id
                    FROM StrTable st
                        INNER JOIN #Items items ON st.Id = items.Value
                ");

                var result = q.ToList();

                Assert.NotEmpty(result);
                Assert.Equal(TestFixture.KnownStrings.Count(), result.Count());
                Assert.Equal(TestFixture.KnownStrings.OrderBy(a => a), result.OrderBy(a => a));
            }
        }

        [Fact]
        public async Task ConnectionWithSet_CompositeIdMapping_AllSelectedValues()
        {
            var mapping = TestFixture.GetCompositeIdMapping();
            using (var conn = new ConnectionWithSet<CompositeId>(TestFixture.CreateSut(), mapping))
            {
                await conn.OpenAsync(TestFixture.KnownCompositeIds);

                var q = await conn.QueryAsync<CompositeId>(@"
                    SELECT tcit.GuidVal, tcit.IntVal
                    FROM CompositeIdTable tcit
                        INNER JOIN #TestCompositeIdSet items ON tcit.GuidVal = items.GuidVal
                            AND tcit.IntVal= items.IntVal
                ");

                var result = q.ToList();

                Assert.NotEmpty(result);
                Assert.Equal(TestFixture.KnownCompositeIds.Count(), result.Count());
                Assert.Equal(TestFixture.KnownCompositeIds.OrderBy(a => a.GuidVal), result.OrderBy(a => a.GuidVal));
            }
        }
    }
}
