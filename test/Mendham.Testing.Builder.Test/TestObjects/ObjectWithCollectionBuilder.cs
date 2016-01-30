using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    public class ObjectWithCollectionBuilder : Builder<ObjectWithCollection>
    {
        int itemCount;

        public ObjectWithCollectionBuilder()
        {
            itemCount = 5;
        }

        public ObjectWithCollectionBuilder WithItemCount(int itemCount)
        {
            this.itemCount = itemCount;
            return this;
        }

        protected override ObjectWithCollection BuildObject()
        {
            return new ObjectWithCollection
            {
                Collection = ObjectCreationContext.CreateMany<int>(itemCount)
            };
        }
    }
}
