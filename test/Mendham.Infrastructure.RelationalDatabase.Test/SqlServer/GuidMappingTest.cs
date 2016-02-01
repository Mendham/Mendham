using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mendham.Infrastructure.RelationalDatabase.Test.Fixtures;
using Xunit;
using Mendham.Infrastructure.RelationalDatabase.SqlServer.BuiltInMapping;
using FluentAssertions;

namespace Mendham.Infrastructure.RelationalDatabase.Test.SqlServer
{
    public class GuidMappingTest : MendhamDatabaseTest
    {
        private readonly GuidMapping sut;

        public GuidMappingTest(DatabaseFixture fixture) : base(fixture)
        {
            sut = new GuidMapping("#Items", "Value");
        }

        [Fact]
        public async Task LoadingData_KnownSet_CreatedTable()
        {
            using (var conn = await Fixture.GetOpenConnectionAsync())
            {
                await conn.LoadDataAsync(Fixture.KnownGuids, sut);

                var result = await conn.ExecuteScalarAsync<bool>(@"
                    IF OBJECT_ID('tempdb..#Items') IS NOT NULL SELECT 1 ELSE SELECT 0");

                result.Should()
                    .BeTrue();
            }
        }

        [Fact]
        public async Task LoadingData_KnownSet_HasCorrectCount()
        {
            using (var conn = await Fixture.GetOpenConnectionAsync())
            {
                await conn.LoadDataAsync(Fixture.KnownGuids, sut);

                var result = await conn.ExecuteScalarAsync<int>(@"
                    SELECT COUNT(1)
                    FROM GuidTable gt
                        INNER JOIN #Items items ON gt.Id = items.Value");

                result.Should()
                    .Be(Fixture.KnownGuids.Count());
            }
        }

        [Fact]
        public async Task DropData_IsDropped_DoesNotExist()
        {
            using (var conn = await Fixture.GetOpenConnectionAsync())
            {
                await conn.LoadDataAsync(Fixture.KnownGuids, sut);
                await conn.DropDataAsync(sut);

                var result = await conn.ExecuteScalarAsync<bool>(@"
                    IF OBJECT_ID('tempdb..#Items') IS NOT NULL SELECT 1 ELSE SELECT 0");

                result.Should()
                    .BeFalse();
            }
        }
    }
}
