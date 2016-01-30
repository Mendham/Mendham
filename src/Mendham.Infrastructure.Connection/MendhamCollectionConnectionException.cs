using System;
using System.Data;

namespace Mendham.Infrastructure.Connection
{
    public abstract class MendhamCollectionConnectionException : Exception
    {
        private const string DEFAULT_MSG = "There was an exception within ConnectionWithSet. See INNER EXCEPTION for details.";

        public MendhamCollectionConnectionException()
        { }

        public MendhamCollectionConnectionException(Exception innerException)
            : base(DEFAULT_MSG, innerException)
        { }
    }

    public class FailureToOpenConnectionWithSetException : MendhamCollectionConnectionException
    {
        protected FailureToOpenConnectionWithSetException()
        { }

        public FailureToOpenConnectionWithSetException(Exception innerException)
            : base(innerException)
        { }

        public override string Message
        {
            get
            {
                return "ConnectionWithSet failed to open. See INNER EXCEPTION for details.";
            }
        }
    }

    public class AttemptedToOpenNonClosedConnectionWithSetException : FailureToOpenConnectionWithSetException
    {
        private readonly ConnectionState _connectionState;

        public AttemptedToOpenNonClosedConnectionWithSetException(ConnectionState connectionState)
        {
            this._connectionState = connectionState;
        }

        public ConnectionState CurrentConnectionState
        {
            get
            {
                return _connectionState;
            }
        }

        public override string Message
        {
            get
            {
                return $"Attempt to open ConnectionWithState while it was in an invalid state ({_connectionState.ToString()}).";
            }
        }
    }
}