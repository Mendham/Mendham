using System;
using System.Data;

namespace Mendham.Infrastructure.RelationalDatabase.Exceptions
{
    public abstract class PreloadedItemConnectionException : Exception
    {
        private const string DEFAULT_MSG = "There was an exception within ConnectionWithSet. See INNER EXCEPTION for details.";

        public PreloadedItemConnectionException()
        { }

        public PreloadedItemConnectionException(Exception innerException)
            : base(DEFAULT_MSG, innerException)
        { }
    }

    public class FailedToDropPreloadedDataException : PreloadedItemConnectionException
    {
        public ConnectionState CurrentState { get; private set; }

        public FailedToDropPreloadedDataException(ConnectionState currentState)
        {
            CurrentState = currentState;
        }

        public override string Message
        {
            get
            {
                return "PreloadedItemConnection failed to drop preloaded table. Was it dropped prior to closing the connection?";
            }
        }
    }

    public class FailedToOpenPreloadedItemsConnectionException : PreloadedItemConnectionException
    {
        protected FailedToOpenPreloadedItemsConnectionException()
        { }

        public FailedToOpenPreloadedItemsConnectionException(Exception innerException)
            : base(innerException)
        { }

        public override string Message
        {
            get
            {
                return "PreloadedItemConnection failed to open. See INNER EXCEPTION for details.";
            }
        }
    }
}