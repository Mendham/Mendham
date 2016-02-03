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

            using (var sut = await connectionFactory.GetOpenConnectionAsync())
            {
                sut.State.Should().Be(ConnectionState.Open);
            }
        }

        [Fact]
        public void GetOpenConnection_ValidConnection_IsOpen()
        {
            var connectionFactory = Fixture.GetConnectionFactory();

            using (var sut = connectionFactory.GetOpenConnection())
            {
                sut.State.Should().Be(ConnectionState.Open);
            }
        }
    }
}
