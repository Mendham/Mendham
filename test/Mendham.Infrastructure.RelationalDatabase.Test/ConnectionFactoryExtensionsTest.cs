using Dapper;
using FluentAssertions;
using Mendham.Infrastructure.RelationalDatabase.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Infrastructure.RelationalDatabase.Test
{
    public class ConnectionFactoryExtensionsTest : MendhamDatabaseTest
    {
        public ConnectionFactoryExtensionsTest(DatabaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetOpenConnectionAsync_ValidConnection_IsOpen()
        {
            var connectionFactory = Fixture.GetConnectionFactory();

            using (var connection = await connectionFactory.GetOpenConnectionAsync())
            {
                connection.State.Should().Be(ConnectionState.Open);
            }
        }

        [Fact]
        public void GetOpenConnection_ValidConnection_IsOpen()
        {
            var connectionFactory = Fixture.GetConnectionFactory();

            using (var connection = connectionFactory.GetOpenConnection())
            {
                connection.State.Should().Be(ConnectionState.Open);
            }
        }

        [Fact]
        public async Task ExecuteAsync_ScalerAction_Runs()
        {
            var connectionFactory = Fixture.GetConnectionFactory();

            var result = await connectionFactory.ExecuteAsync(conn =>
                conn.ExecuteScalarAsync<bool>("SELECT TOP 1 1 FROM GuidTable"));

            result.Should().BeTrue();
        }

        [Fact]
        public void Execute_ScalerAction_Runs()
        {
            var connectionFactory = Fixture.GetConnectionFactory();

            var result = connectionFactory.Execute(conn =>
                conn.ExecuteScalar<bool>("SELECT TOP 1 1 FROM GuidTable"));

            result.Should().BeTrue();
        }
    }
}
