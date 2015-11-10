using Mendham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency
{
	public class ByteConcurrencyToken : IConcurrencyToken
    {
		public ByteConcurrencyToken(byte[] tokenValue)
		{
			tokenValue.VerifyArgumentNotNullOrEmpty("Token value must be provided");

			this.Value = tokenValue;
		}

		public byte[] Value { get; private set; }

        string IConcurrencyToken.DisplayValue { get { return GetDisplayValue(); } }

        public static implicit operator byte[] (ByteConcurrencyToken token)
		{
			return token.Value;
		}

		public static implicit operator ByteConcurrencyToken(byte[] tokenArray)
		{
			return new ByteConcurrencyToken(tokenArray);
		}

		public override bool Equals(object other)
		{
			var token = other as ByteConcurrencyToken;

			if (token == null)
				return false;

			return this.Equals(token);
		}

		public bool Equals(IConcurrencyToken other)
		{
            var otherCt = other as ByteConcurrencyToken;

            if (otherCt == default(ByteConcurrencyToken))
                return false;

			return this.Value.SequenceEqual(otherCt.Value);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (int)BitConverter.ToInt64(this.Value, 0);
			}
		}

		public override string ToString()
		{
            return string.Format("Byte Currency Token [0x{0:X}]", GetDisplayValue());
		}

        private string GetDisplayValue()
        {
            var val = BitConverter.ToInt64(this.Value.Reverse().ToArray(), 0);
            return string.Format("0x{0:X}", val);
        }
    }
}