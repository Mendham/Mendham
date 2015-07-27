using Dapper;
using Mendham.Infrastructure.Dapper.Test.Fixtures;
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
            using (var conn = new ConnectionWithSet<int>(_fixture.CreateSut(), mapping))
            {
                await conn.OpenAsync(_fixture.KnownInts);

                var q = await conn.QueryAsync<int>(@"
                    SELECT Id
                    FROM IntTable it
                        INNER JOIN #Items items ON it.Id = items.Value
                ");

                var result = q.ToList();

                Assert.NotEmpty(result);
                Assert.Equal(_fixture.KnownInts.Count(), result.Count());
                Assert.Equal(_fixture.KnownInts.OrderBy(a => a), result.OrderBy(a => a));
            }
        }

        [Fact]
        public async Task ConnectionWithSet_GuidDefaultMapping_AllSelectedValues()
        {
            var mapping = DefaultConnectionWithSetMapping.Get<Guid>();
            using (var conn = new ConnectionWithSet<Guid>(_fixture.CreateSut(), mapping))
            {
                await conn.OpenAsync(_fixture.KnownGuids);

                var q = await conn.QueryAsync<Guid>(@"
                    SELECT Id
                    FROM GuidTable gt
                        INNER JOIN #Items items ON gt.Id = items.Value
                ");

                var result = q.ToList();

                Assert.NotEmpty(result);
                Assert.Equal(_fixture.KnownGuids.Count(), result.Count());
                Assert.Equal(_fixture.KnownGuids.OrderBy(a => a), result.OrderBy(a => a));
            }
        }

        [Fact]
        public async Task ConnectionWithSet_StringDefaultMapping_AllSelectedValues()
        {
            var mapping = DefaultConnectionWithSetMapping.Get<string>();
            using (var conn = new ConnectionWithSet<string>(_fixture.CreateSut(), mapping))
            {
                await conn.OpenAsync(_fixture.KnownStrings);

                var q = await conn.QueryAsync<string>(@"
                    SELECT Id
                    FROM StrTable st
                        INNER JOIN #Items items ON st.Id = items.Value
                ");

                var result = q.ToList();

                Assert.NotEmpty(result);
                Assert.Equal(_fixture.KnownStrings.Count(), result.Count());
                Assert.Equal(_fixture.KnownStrings.OrderBy(a => a), result.OrderBy(a => a));
            }
        }

    }
}
