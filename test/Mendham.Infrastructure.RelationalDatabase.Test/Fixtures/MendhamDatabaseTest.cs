using Mendham.Infrastructure.RelationalDatabase.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Infrastructure.RelationalDatabase.Test.Fixtures
{
    [Collection(DatabaseCollection.CollectionName)]
    public abstract class MendhamDatabaseTest
    {
        private readonly DatabaseFixture fixture;

        public MendhamDatabaseTest(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        public DatabaseFixture Fixture
        {
            get { return fixture; }
        }
    }
}
