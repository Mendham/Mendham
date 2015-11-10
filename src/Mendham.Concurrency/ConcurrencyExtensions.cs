using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency
{
    public static class ConcurrencyExtensions
    {
		public static T VerifyConcurrencyTokenIsSet<T>(this T obj, string message = null)
			where T : IHasConcurrencyToken
		{
			return obj.VerifyTokenObjectIsNotNull()
				.VerifyArgumentMeetsCriteria(a => a.Token != null, message ?? "Concurrency Token is not set");
		}

		public static T VerifyConcurrencyTokenIsNotSet<T>(this T obj, string message = null)
			where T : IHasConcurrencyToken
		{
			return obj.VerifyArgumentMeetsCriteria(a => a.Token == null, message ?? "Concurrency Token is already set");
		}

		public static T ValidateConcurrencyToken<T>(this T obj, IConcurrencyToken serverToken, string message = null)
			where T : IHasConcurrencyToken
		{
			obj.VerifyConcurrencyTokenIsSet();

			if (!obj.Token.Equals(serverToken))
				throw new ConcurrencyException(obj.Token, serverToken, message);

			return obj;
		}

		public static T SetConcurrencyToken<T>(this T obj, IConcurrencyToken newToken)
		   where T : IHasConcurrencyToken
		{
            newToken.VerifyArgumentNotDefaultValue("Token is required");

            obj.Token = newToken;
			return obj;
		}

		private static T VerifyTokenObjectIsNotNull<T>(this T obj)
		{
			return obj.VerifyArgumentNotNull("Object containing token is null");
		}
	}
}
