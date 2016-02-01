using Dapper;
using FluentAssertions;
using Mendham.Infrastructure.RelationalDatabase.SqlServer.BuiltInMapping;
using Mendham.Infrastructure.RelationalDatabase.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Infrastructure.RelationalDatabase.Test.SqlServer
{
    public class IntMappingTest : MendhamDatabaseTest
    {
        private readonly IntMapping sut;

        public IntMappingTest(DatabaseFixture fixture) : base(fixture)
        {
            sut = new IntMapping("#Items", "Value");
        }

        [Fact]
        public async Task LoadingData_KnownSet_CreatedTable()
        {
            using (var conn = await Fixture.GetOpenConnectionAsync())
            {
                await conn.LoadDataAsync(Fixture.KnownInts, sut);

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
                await conn.LoadDataAsync(Fixture.KnownInts, sut);

                var result = await conn.ExecuteScalarAsync<int>(@"SELECT COUNT(1) FROM #Items");

                result.Should()
                    .Be(Fixture.KnownInts.Count());
            }
        }

        [Fact]
        public async Task DropData_IsDropped_DoesNotExist()
        {
            using (var conn = await Fixture.GetOpenConnectionAsync())
            {
                await conn.LoadDataAsync(Fixture.KnownInts, sut);
                await conn.DropDataAsync(sut);

                var result = await conn.ExecuteScalarAsync<bool>(@"
                    IF OBJECT_ID('tempdb..#Items') IS NOT NULL SELECT 1 ELSE SELECT 0");

                result.Should()
                    .BeFalse();
            }
        }
    }
}
