using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency
{
	public class ConcurrencyToken : IEquatable<ConcurrencyToken>
	{
		public ConcurrencyToken(byte[] tokenValue)
		{
			tokenValue.VerifyArgumentNotNullOrEmpty("Token value must be provided");

			this.Value = tokenValue;
		}

		public byte[] Value { get; private set; }

		public static implicit operator byte[] (ConcurrencyToken token)
		{
			return token.Value;
		}

		public static implicit operator ConcurrencyToken(byte[] tokenArray)
		{
			return new ConcurrencyToken(tokenArray);
		}

		public override bool Equals(object other)
		{
			var token = other as ConcurrencyToken;

			if (token == null)
				return false;

			return this.Equals(token);
		}

		public bool Equals(ConcurrencyToken other)
		{
			return this.Value.SequenceEqual(other.Value);
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
			var val = BitConverter.ToInt64(this.Value.Reverse().ToArray(), 0);
			return String.Format("Currency Token [0x{0:X}]", val);
		}
	}
}