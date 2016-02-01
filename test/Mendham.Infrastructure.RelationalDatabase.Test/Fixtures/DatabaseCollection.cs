using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Infrastructure.RelationalDatabase.Test.Fixtures
{
    [CollectionDefinition(CollectionName)]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        public const string CollectionName = "Database Collection";
    }
}
