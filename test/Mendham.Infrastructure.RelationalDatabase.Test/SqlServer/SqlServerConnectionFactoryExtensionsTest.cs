﻿using Dapper;
using FluentAssertions;
using Mendham.Infrastructure.RelationalDatabase.Exceptions;
using Mendham.Infrastructure.RelationalDatabase.SqlServer;
using Mendham.Infrastructure.RelationalDatabase.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Infrastructure.RelationalDatabase.Test.SqlServer
{
    public class SqlServerConnectionFactoryExtensionsTest : MendhamDatabaseTest
    {
        private const string CUSTOM_TABLE = "#CustomItems";
        private const string CUSTOM_COLUMN = "CustomColumn";

        public SqlServerConnectionFactoryExtensionsTest(DatabaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void GetPreloadedItemConnection_Int_ClosedConnection()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = factory.GetPreloadedItemConnection(Fixture.KnownInts))
            {
                conn.State.Should()
                    .Be(ConnectionState.Closed, "the connection was not opened");
                conn.Items.Should()
                    .BeEquivalentTo(Fixture.KnownInts);
            }
        }

        [Fact]
        public async Task GetPreloadedItemConnection_Int_UsesCorrectTableAndColumn()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = factory.GetPreloadedItemConnection(Fixture.KnownInts, CUSTOM_TABLE, CUSTOM_COLUMN))
            {
                await conn.OpenAsync();

                var result = await conn.ExecuteScalarAsync<int>(@"
                    SELECT COUNT(1)
                    FROM IntTable it
                        INNER JOIN #CustomItems items ON it.Id = items.CustomColumn");

                result.Should()
                    .Be(Fixture.KnownInts.Count());
            }
        }

        [Fact]
        public async Task GetOpenPreloadedItemConnectionAsync_Int_OpenConnectionWithItems()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = await factory.GetOpenPreloadedItemConnectionAsync(Fixture.KnownInts, CUSTOM_TABLE, CUSTOM_COLUMN))
            {
                var result = await conn.ExecuteScalarAsync<int>(@"
                    SELECT COUNT(1)
                    FROM IntTable it
                        INNER JOIN #CustomItems items ON it.Id = items.CustomColumn");

                conn.State.Should()
                    .Be(ConnectionState.Open, "the connection was opened");
                conn.Items.Should()
                    .BeEquivalentTo(Fixture.KnownInts);
                result.Should()
                    .Be(Fixture.KnownInts.Count());
            }
        }

        [Fact]
        public void GetOpenPreloadedItemConnection_Int_OpenConnectionWithItems()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = factory.GetOpenPreloadedItemConnection(Fixture.KnownInts, CUSTOM_TABLE, CUSTOM_COLUMN))
            {
                var result = conn.ExecuteScalar<int>(@"
                    SELECT COUNT(1)
                    FROM IntTable it
                        INNER JOIN #CustomItems items ON it.Id = items.CustomColumn");

                conn.State.Should()
                    .Be(ConnectionState.Open, "the connection was opened");
                conn.Items.Should()
                    .BeEquivalentTo(Fixture.KnownInts);
                result.Should()
                    .Be(Fixture.KnownInts.Count());
            }
        }

        [Fact]
        public void GetPreloadedItemConnection_Guid_ClosedConnection()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = factory.GetPreloadedItemConnection(Fixture.KnownGuids))
            {
                conn.State.Should()
                    .Be(ConnectionState.Closed, "the connection was not opened");
                conn.Items.Should()
                    .BeEquivalentTo(Fixture.KnownGuids);
            }
        }

        [Fact]
        public async Task GetPreloadedItemConnection_Guid_UsesCorrectTableAndColumn()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = factory.GetPreloadedItemConnection(Fixture.KnownGuids, CUSTOM_TABLE, CUSTOM_COLUMN))
            {
                await conn.OpenAsync();

                var result = await conn.ExecuteScalarAsync<int>(@"
                    SELECT COUNT(1)
                    FROM GuidTable it
                        INNER JOIN #CustomItems items ON it.Id = items.CustomColumn");

                result.Should()
                    .Be(Fixture.KnownGuids.Count());
            }
        }

        [Fact]
        public async Task GetOpenPreloadedItemConnectionAsync_Guid_OpenConnectionWithItems()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = await factory.GetOpenPreloadedItemConnectionAsync(Fixture.KnownGuids, CUSTOM_TABLE, CUSTOM_COLUMN))
            {
                var result = await conn.ExecuteScalarAsync<int>(@"
                    SELECT COUNT(1)
                    FROM GuidTable it
                        INNER JOIN #CustomItems items ON it.Id = items.CustomColumn");

                conn.State.Should()
                    .Be(ConnectionState.Open, "the connection was opened");
                conn.Items.Should()
                    .BeEquivalentTo(Fixture.KnownGuids);
                result.Should()
                    .Be(Fixture.KnownGuids.Count());
            }
        }

        [Fact]
        public void GetOpenPreloadedItemConnection_Guid_OpenConnectionWithItems()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = factory.GetOpenPreloadedItemConnection(Fixture.KnownGuids, CUSTOM_TABLE, CUSTOM_COLUMN))
            {
                var result = conn.ExecuteScalar<int>(@"
                    SELECT COUNT(1)
                    FROM GuidTable it
                        INNER JOIN #CustomItems items ON it.Id = items.CustomColumn");

                conn.State.Should()
                    .Be(ConnectionState.Open, "the connection was opened");
                conn.Items.Should()
                    .BeEquivalentTo(Fixture.KnownGuids);
                result.Should()
                    .Be(Fixture.KnownGuids.Count());
            }
        }

        [Fact]
        public void GetPreloadedItemConnection_String_ClosedConnection()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = factory.GetPreloadedItemConnection(Fixture.KnownStrings))
            {
                conn.State.Should()
                    .Be(ConnectionState.Closed, "the connection was not opened");
                conn.Items.Should()
                    .BeEquivalentTo(Fixture.KnownStrings);
            }
        }

        [Fact]
        public async Task GetPreloadedItemConnection_String_UsesCorrectTableAndColumn()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = factory.GetPreloadedItemConnection(Fixture.KnownStrings, CUSTOM_TABLE, CUSTOM_COLUMN))
            {
                await conn.OpenAsync();

                var result = await conn.ExecuteScalarAsync<int>(@"
                    SELECT COUNT(1)
                    FROM StrTable it
                        INNER JOIN #CustomItems items ON it.Id = items.CustomColumn");

                result.Should()
                    .Be(Fixture.KnownStrings.Count());
            }
        }

        [Fact]
        public async Task GetOpenPreloadedItemConnectionAsync_String_OpenConnectionWithItems()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = await factory.GetOpenPreloadedItemConnectionAsync(Fixture.KnownStrings, CUSTOM_TABLE, CUSTOM_COLUMN))
            {
                var result = await conn.ExecuteScalarAsync<int>(@"
                    SELECT COUNT(1)
                    FROM StrTable it
                        INNER JOIN #CustomItems items ON it.Id = items.CustomColumn");

                conn.State.Should()
                    .Be(ConnectionState.Open, "the connection was opened");
                conn.Items.Should()
                    .BeEquivalentTo(Fixture.KnownStrings);
                result.Should()
                    .Be(Fixture.KnownStrings.Count());
            }
        }

        [Fact]
        public void GetOpenPreloadedItemConnection_String_OpenConnectionWithItems()
        {
            var factory = Fixture.GetConnectionFactory();

            using (var conn = factory.GetOpenPreloadedItemConnection(Fixture.KnownStrings, CUSTOM_TABLE, CUSTOM_COLUMN))
            {
                var result = conn.ExecuteScalar<int>(@"
                    SELECT COUNT(1)
                    FROM StrTable it
                        INNER JOIN #CustomItems items ON it.Id = items.CustomColumn");

                conn.State.Should()
                    .Be(ConnectionState.Open, "the connection was opened");
                conn.Items.Should()
                    .BeEquivalentTo(Fixture.KnownStrings);
                result.Should()
                    .Be(Fixture.KnownStrings.Count());
            }
        }

        [Fact]
        public void GetPreloadedItemConnection_StringInvalidLength_ThrowsArguementOutOfRangeException()
        {
            var factory = Fixture.GetConnectionFactory();
            int maxCharacters = 9;

            // Over 10 characters 
            var invalidValue = "12345678_0";

            var valuesToPass = new List<string>
                {
                    "ABC",
                    "123",
                    invalidValue,
                    "XYZ"
                };

            Action act = () => factory.GetOpenPreloadedItemConnection(valuesToPass, CUSTOM_TABLE,
                CUSTOM_COLUMN, maxCharacters);

            act.ShouldThrow<AttemptedToLoadInvalidItemException>()
                    .Where(a => Equals(a.FirstInvalidItem, invalidValue))
                    .Where(a => a.Message.Contains(invalidValue))
                    .Where(a => a.Message.Contains("9"));
        }
    }
}
