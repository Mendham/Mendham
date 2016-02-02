using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.RelationalDatabase.Exceptions
{
    public abstract class ItemLoaderException : Exception
    {
    }

    public class AttemptedToLoadInvalidItemException : ItemLoaderException
    {
        private readonly object _firstInvalidItem;
        private readonly string messageFromMapping;

        public AttemptedToLoadInvalidItemException(object firstInvalidItem, string messageFromMapping)
        {
            _firstInvalidItem = firstInvalidItem;
            this.messageFromMapping = messageFromMapping;
        }

        public object FirstInvalidItem { get { return _firstInvalidItem; } }

        public override string Message
        {
            get
            {
                return $"One or more items could not be loaded for the following reason: \"{messageFromMapping}\" \r\nFirst Invalid Item: {_firstInvalidItem}";
            }
        }

        internal static AttemptedToLoadInvalidItemException BuildException<T>(IEnumerable<T> items, IItemLoaderMapping<T> mapping)
        {
            var firstInvalid = items
                .FirstOrDefault(a => !mapping.ItemIsValidPredicate(a));

            return new AttemptedToLoadInvalidItemException(firstInvalid, mapping.InvalidSetErrorMessage);
        }
    }
}
