using System;
using System.Linq;

namespace Mendham.Concurrency
{
    public class Int64ConcurrencyToken : IConcurrencyToken, IComparable
    {
        private readonly long tokenValue;
        private readonly byte[] byteValue;

		public Int64ConcurrencyToken(byte[] byteValue)
		{
            byteValue
                .VerifyArgumentNotNullOrEmpty(nameof(byteValue), "Token value must be provided")
                .VerifyArgumentMeetsCriteria(a => a.Count() == 8, nameof(byteValue), "Value must have exactly 8 bytes")
                .VerifyArgumentMeetsCriteria(a => !a.All(b => b == 0), nameof(byteValue), "Value must not be all zeros");

            this.byteValue = byteValue;
            this.tokenValue = BitConverter.ToInt64(byteValue.Reverse().ToArray(), 0);
		}

        public Int64ConcurrencyToken(long tokenValue)
        {
            this.tokenValue = tokenValue
                .VerifyArgumentNotDefaultValue(nameof(tokenValue), "Token value cannot be zero");

            this.byteValue = BitConverter.GetBytes(tokenValue).Reverse().ToArray();
            this.tokenValue = tokenValue;
        }

        /// <summary>
        /// Int64 value for concurrency token
        /// </summary>
        public long Value
        {
            get { return tokenValue; }
        }

        /// <summary>
        /// Byte[] value of the concurrency token
        /// </summary>
        public byte[] Bytes
        {
            get { return byteValue; }
        }

        string IConcurrencyToken.DisplayValue { get { return GetDisplayValue(); } }

        public static implicit operator byte[] (Int64ConcurrencyToken token)
		{
			return token.Bytes;
		}

		public static implicit operator Int64ConcurrencyToken(byte[] tokenArray)
		{
			return new Int64ConcurrencyToken(tokenArray);
		}

		public override bool Equals(object other)
		{
            if (ReferenceEquals(this, other))
                return true;

            var otherInt64ConcurrencyToken = other as Int64ConcurrencyToken;

			if (otherInt64ConcurrencyToken == default(Int64ConcurrencyToken))
				return false;

            return Value.Equals(otherInt64ConcurrencyToken.Value);
		}

		public bool Equals(IConcurrencyToken other)
		{
            if (ReferenceEquals(this, other))
                return true;

            var otherInt64ConcurrencyToken = other as Int64ConcurrencyToken;

            if (otherInt64ConcurrencyToken == default(Int64ConcurrencyToken))
                return false;

            return Value.Equals(otherInt64ConcurrencyToken.Value);
		}

		public override int GetHashCode()
		{
            return tokenValue.GetHashCode();
        }

        public override string ToString()
		{
            return $"Int64ConcurrencyToken [{GetDisplayValue()}]";
        }

        private string GetDisplayValue()
        {
            return string.Format("0x{0:X}", Value);
        }

        public int CompareTo(object obj)
        {
            var otherInt64ConcurrencyToken = obj as Int64ConcurrencyToken;

            if (otherInt64ConcurrencyToken == default(Int64ConcurrencyToken))
                return 1;

            return Value.CompareTo(otherInt64ConcurrencyToken.Value);
        }

        /// <summary>
        /// Compares this Int64ConcurrencyToken with other to determine if it is at least as current (equal or
        /// more recent) than the other Int64ConcurrencyToken
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if the values are the same or if this concurrency token is newer.  False if older.</returns>
        public bool IsAtLeastAsCurrentAs(Int64ConcurrencyToken other)
        {
            return Value.CompareTo(other.Value) >= 0;
        }
    }
}