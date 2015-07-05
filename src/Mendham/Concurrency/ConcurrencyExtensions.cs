using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency
{
    public static class ConcurrencyExtensions
    {
		public static T VerifyConcurrencyTokenIsNotNull<T>(this T obj, String message = null)
			where T : IHasConcurrencyToken
		{
			return obj.VerifyTokenObjectIsNotNull()
				.VerifyArgumentMeetsCriteria(a => a.Token != null, message ?? "Concurrency Token is null");
		}

		public static T VerifyConcurrencyTokenIsNotSet<T>(this T obj, String message = null)
			where T : IHasConcurrencyToken
		{
			return obj.VerifyArgumentMeetsCriteria(a => a.Token == null, message ?? "Concurrency Token is already set");
		}

		public static T ValidateConcurrencyToken<T>(this T obj, ConcurrencyToken serverToken, String message = null)
			where T : IHasConcurrencyToken
		{
			obj.VerifyConcurrencyTokenIsNotNull();

			if (!obj.Token.Equals(serverToken))
				throw new ConcurrencyException(obj.Token, serverToken, message);

			return obj;
		}

		public static T ValidateConcurrencyToken<T>(this T obj, IEnumerable<byte[]> tokenFromQuery, string message = null)
			where T : IHasConcurrencyToken
		{
			return obj.ValidateConcurrencyToken(GetTokenFromCollection(tokenFromQuery), message);
		}

		private static ConcurrencyToken GetTokenFromCollection(IEnumerable<byte[]> tokenFromQuery)
		{
			var value = tokenFromQuery.SingleOrDefault();

			if (value == null || !value.Any())
				throw new InvalidOperationException("Token not provided by query");

			return value;
		}

		public static T SetConcurrencyToken<T>(this T obj, ConcurrencyToken newToken)
		   where T : IHasConcurrencyToken
		{
			obj.Update(newToken);
			return obj;
		}

		private static T VerifyTokenObjectIsNotNull<T>(this T obj)
		{
			return obj.VerifyArgumentNotNull("Object containing token is null");
		}

		private static T SetOrOverwriteConcurrencyToken<T>(this T obj, ConcurrencyToken serverFromToken)
			where T : IHasConcurrencyToken
		{
			obj.Update(serverFromToken);
			return obj;
		}
	}
}
