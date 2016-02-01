using Dapper;
using Mendham.Infrastructure.RelationalDatabase.SqlServer;
using Mendham.Infrastructure.RelationalDatabase.Test.Fixtures;
using Mendham.Infrastructure.RelationalDatabase.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Infrastructure.RelationalDatabase.Test
{
    public class PreloatedItemCollectionTest : MendhamDatabaseTest
    {
        public PreloatedItemCollectionTest(DatabaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task ConnectionWithSet_IntSet_AllSelectedValues()
        {
            var sut = Fixture.GetConnectionFactory();

            using (var conn = await sut.GetOpenPreloadedItemConnectionAsync(Fixture.KnownInts))
            {
                var q = await conn.QueryAsync<int>(@"
                    SELECT Id
                    FROM IntTable it
                        INNER JOIN #Items items ON it.Id = items.Value
                ");

                var result = q.ToList();

                Assert.NotEmpty(result);
                Assert.Equal(Fixture.KnownInts.Count(), result.Count());
                Assert.Equal(Fixture.KnownInts.OrderBy(a => a), result.OrderBy(a => a));
            }
        }

        [Fact]
        public async Task ConnectionWithSet_GuidSet_AllSelectedValues()
        {
            var sut = Fixture.GetConnectionFactory();

            using (var conn = await sut.GetOpenPreloadedItemConnectionAsync(Fixture.KnownGuids))
            {
                var q = await conn.QueryAsync<Guid>(@"
                    SELECT Id
                    FROM GuidTable gt
                        INNER JOIN #Items items ON gt.Id = items.Value
                ");

                var result = q.ToList();

                Assert.NotEmpty(result);
                Assert.Equal(Fixture.KnownGuids.Count(), result.Count());
                Assert.Equal(Fixture.KnownGuids.OrderBy(a => a), result.OrderBy(a => a));
            }
        }

        [Fact]
        public async Task ConnectionWithSet_StringSet_AllSelectedValues()
        {
            var sut = Fixture.GetConnectionFactory();

            using (var conn = await sut.GetOpenPreloadedItemConnectionAsync(Fixture.KnownStrings))
            {
                var q = await conn.QueryAsync<string>(@"
                    SELECT Id
                    FROM StrTable st
                        INNER JOIN #Items items ON st.Id = items.Value
                ");

                var result = q.ToList();

                Assert.NotEmpty(result);
                Assert.Equal(Fixture.KnownStrings.Count(), result.Count());
                Assert.Equal(Fixture.KnownStrings.OrderBy(a => a), result.OrderBy(a => a));
            }
        }

        [Fact]
        public async Task ConnectionWithSet_CompositeIdMapping_AllSelectedValues()
        {
            var sut = Fixture.GetConnectionFactory();
            var mapping = Fixture.GetCompositeIdMapping();

            using (var conn = await sut.GetOpenPreloadedItemConnectionAsync(Fixture.KnownCompositeIds, mapping))
            {
                var q = await conn.QueryAsync<CompositeId>(@"
                    SELECT tcit.GuidVal, tcit.IntVal
                    FROM CompositeIdTable tcit
                        INNER JOIN #TestCompositeIdSet items ON tcit.GuidVal = items.GuidVal
                            AND tcit.IntVal= items.IntVal
                ");

                var result = q.ToList();

                Assert.NotEmpty(result);
                Assert.Equal(Fixture.KnownCompositeIds.Count(), result.Count());
                Assert.Equal(Fixture.KnownCompositeIds.OrderBy(a => a.GuidVal), result.OrderBy(a => a.GuidVal));
            }
        }
    }
}
