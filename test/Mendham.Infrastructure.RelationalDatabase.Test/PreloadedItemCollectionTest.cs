using Dapper;
using FluentAssertions;
using Mendham.Infrastructure.RelationalDatabase.Exceptions;
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
        public void ConnectionWithSet_CompositeIdMapping_AllSelectedValues()
        {
            var sut = Fixture.GetConnectionFactory();
            var mapping = Fixture.GetCompositeIdMapping();

            using (var conn = sut.GetOpenPreloadedItemConnection(Fixture.KnownCompositeIds, mapping))
            {
                var q = conn.Query<CompositeId>(@"
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

        [Fact]
        public async Task ConnectionWithSetAsync_CompositeIdMapping_AllSelectedValues()
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

        [Fact]
        public async Task OpenAsync_CalledTwice_ThrowsFailedToOpenPreloadedItemsConnectionException()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = factory.GetPreloadedItemConnection(Fixture.KnownInts))
            {
                await conn.OpenAsync();

                Func<Task> act = async () => await conn.OpenAsync();

                act.ShouldThrow<FailedToOpenPreloadedItemsConnectionException>();
            }
        }

        [Fact]
        public void Open_CalledTwice_ThrowsFailedToOpenPreloadedItemsConnectionException()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = factory.GetPreloadedItemConnection(Fixture.KnownInts))
            {
                conn.Open();

                Action act = () => conn.Open();

                act.ShouldThrow<FailedToOpenPreloadedItemsConnectionException>();
            }
        }

        [Fact]
        public async Task CloseAsync_AlreadyDropedPreloadedTable_ThrowsFailedToDropPreloadedDataException()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = factory.GetPreloadedItemConnection(Fixture.KnownInts, "#TestItems"))
            {
                await conn.OpenAsync();

                await conn.ExecuteAsync("DROP TABLE #TestItems");

                await Assert.ThrowsAsync<FailedToDropPreloadedDataException>(() => conn.CloseAsync());
            }
        }

        [Fact]
        public void Close_AlreadyDropedPreloadedTable_ThrowsFailedToDropPreloadedDataException()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = factory.GetPreloadedItemConnection(Fixture.KnownInts, "#TestItems"))
            {
                conn.Open();

                conn.Execute("DROP TABLE #TestItems");

                Action act = () => conn.Close();

                act.ShouldThrow<FailedToDropPreloadedDataException>();
            }
        }
    }
}
