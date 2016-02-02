using Dapper;
using FluentAssertions;
using Mendham.Infrastructure.RelationalDatabase.Exceptions;
using Mendham.Infrastructure.RelationalDatabase.SqlServer.BuiltInMapping;
using Mendham.Infrastructure.RelationalDatabase.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Infrastructure.RelationalDatabase.Test.SqlServer
{
    public class NVarcharMappingTest : MendhamDatabaseTest
    {
        public NVarcharMappingTest(DatabaseFixture fixture) : base(fixture)
        { }

        [Fact]
        public async Task LoadingData_KnownSet_CreatedTable()
        {
            var sut = new NVarcharMapping("#Items", "Value", 10);

            using (var conn = await Fixture.GetOpenConnectionAsync())
            {
                await conn.LoadDataAsync(Fixture.KnownStrings, sut);

                var result = await conn.ExecuteScalarAsync<bool>(@"
                    IF OBJECT_ID('tempdb..#Items') IS NOT NULL SELECT 1 ELSE SELECT 0");

                result.Should()
                    .BeTrue();
            }
        }

        [Fact]
        public async Task LoadingData_KnownSet_HasCorrectCount()
        {
            var sut = new NVarcharMapping("#Items", "Value", 10);

            using (var conn = await Fixture.GetOpenConnectionAsync())
            {
                await conn.LoadDataAsync(Fixture.KnownStrings, sut);

                var result = await conn.ExecuteScalarAsync<int>(@"
                    SELECT COUNT(1)
                    FROM StrTable st
                        INNER JOIN #Items items ON st.Id = items.Value");

                result.Should()
                    .Be(Fixture.KnownStrings.Count());
            }
        }

        [Fact]
        public async Task DropData_IsDropped_DoesNotExist()
        {
            var sut = new NVarcharMapping("#Items", "Value", 10);

            using (var conn = await Fixture.GetOpenConnectionAsync())
            {
                await conn.LoadDataAsync(Fixture.KnownStrings, sut);
                await conn.DropDataAsync(sut);

                var result = await conn.ExecuteScalarAsync<bool>(@"
                    IF OBJECT_ID('tempdb..#Items') IS NOT NULL SELECT 1 ELSE SELECT 0");

                result.Should()
                    .BeFalse();
            }
        }

        [Fact]
        public async Task LoadingData_InvalidSet_ThrowsArguementOutOfRangeException()
        {
            var sut = new NVarcharMapping("#Items", "Value", 100);

            // Over 100 characters 
            var invalidValue = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var valuesToPass = new List<string>
                {
                    "ABC",
                    "123",
                    invalidValue,
                    "XYZ"
                };

            using (var conn = await Fixture.GetOpenConnectionAsync())
            {
                Func<Task> act = async () => await conn.LoadDataAsync(valuesToPass, sut);

                act.ShouldThrow<AttemptedToLoadInvalidItemException>()
                    .Where(a => Equals(a.FirstInvalidItem, invalidValue))
                    .Where(a => a.Message.Contains("100"));
            }
        }
    }
}
