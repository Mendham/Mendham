using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency
{
    public static class ConcurrencyExtensions
    {
		public static T VerifyConcurrencyTokenIsApplied<T>(this T obj, string message = null)
			where T : IHasConcurrencyToken
		{
            obj.VerifyObjectWithTokenIsNotNull("VerifyConcurrencyTokenIsApplied failed.");

            if (obj.Token != null)
            {
                throw new ConcurrencyTokenAlreadyAppliedException(obj, obj.Token, message);
            }

            return obj;
		}

		public static T VerifyConcurrencyTokenIsNotApplied<T>(this T obj, string message = null)
			where T : IHasConcurrencyToken
		{
            obj.VerifyObjectWithTokenIsNotNull("VerifyConcurrencyTokenIsNotApplied failed.");

            if (obj.Token == null)
            {
                throw new ConcurrencyTokenNotAppliedException(obj, message);
            }

            return obj;
		}

		public static T ValidateConcurrencyToken<T>(this T obj, IConcurrencyToken serverToken, string message = null)
			where T : IHasConcurrencyToken
		{
			obj.VerifyObjectWithTokenIsNotNull("ValidateConcurrencyToken failed")
                .VerifyConcurrencyTokenIsApplied();

			if (!obj.Token.Equals(serverToken))
            {
                throw new InvaildConcurrencyTokenException(obj, obj.Token, serverToken, message);
            }

            return obj;
		}

		public static T SetConcurrencyToken<T>(this T obj, IConcurrencyToken newToken)
		   where T : IHasConcurrencyToken
		{
            obj.VerifyObjectWithTokenIsNotNull("SetConcurrencyToken failed");
            newToken.VerifyArgumentNotDefaultValue(nameof(newToken));

            obj.Token = newToken;
			return obj;
		}

		private static T VerifyObjectWithTokenIsNotNull<T>(this T obj, string message)
            where T : IHasConcurrencyToken
		{
            if (obj == null)
            {
                throw new NullReferenceException($"Object containing token is null. {message}");
            }

            return obj;
		}
	}
}
