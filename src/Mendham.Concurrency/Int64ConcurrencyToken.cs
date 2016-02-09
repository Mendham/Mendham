using Mendham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency
{
	public class Int64ConcurrencyToken : IConcurrencyToken
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

        public long Value
        {
            get { return tokenValue; }
        }

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
            return string.Format("Int64ConcurrencyToken [0x{0:X}]", GetDisplayValue());
		}

        private string GetDisplayValue()
        {
            return string.Format("0x{0:X}", Value);
        }
    }
}