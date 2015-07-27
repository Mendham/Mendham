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
    }
}
