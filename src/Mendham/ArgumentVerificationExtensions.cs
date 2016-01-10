using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Mendham
{
	public static class ArgumentVerificationExtensions
	{
		/// <summary>
		/// Throws an ArgumentException if value is null
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">Value to test</param>
        /// <param name="paramName">Name of parameter</param>
		/// <param name="message">Message to display if value is null (optional)</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static T VerifyArgumentNotNull<T>(this T obj, string paramName, string message = null)
		{
            if (obj != null)
            {
                return obj;
            }
            else if (message == null)
            {
                throw new ArgumentNullException(paramName);
            }
            else
            {
                throw new ArgumentNullException(paramName, message);
            }
		}

        /// <summary>
        /// Throws an ArgumentException if object is null or default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tObj">Value to test</param>
        /// <param name="paramName">Name of parameter</param>
        /// <param name="message">Message to display if value is null or default value (optional)</param>
        /// <returns></returns>
        [DebuggerStepThrough]
		public static T VerifyArgumentNotDefaultValue<T>(this T tObj, string paramName, string message = null)
		{
            if (!Equals(tObj, default(T)))
            {
                return tObj;
            }

            tObj.VerifyArgumentNotNull(paramName, message);

            if (message == null)
            {
                message = $"Parameter '{paramName}' cannot be the default value for type {typeof(T).FullName} ({default(T)}).";
            }

            throw new ArgumentException(message, paramName);
        }

        /// <summary>
        /// Throws an ArgumentException if enumerable is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tEnumerable">Enumerable to test</param>
        /// <param name="paramName">Name of parameter</param>
        /// <param name="message">Message to display if enumerable is null or empty (optional)</param>
        /// <returns></returns>
        [DebuggerStepThrough]
		public static IEnumerable<T> VerifyArgumentNotNullOrEmpty<T>(this IEnumerable<T> tEnumerable, string paramName, string message = null)
		{
            tEnumerable.VerifyArgumentNotNull(paramName, message);

            if (tEnumerable.Any())
            {
                return tEnumerable;
            }
            else if (message == null)
            {
                message = $"Parameter '{paramName}' cannot be empty.";
            }

            throw new ArgumentException(message, paramName);
		}

		/// <summary>
		/// Throws an ArgumentException if string is null or empty
		/// </summary>
		/// <param name="str">String to test</param>
		/// <param name="message">Message to display if string is null or empty</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static string VerifyArgumentNotNullOrEmpty(this string str, string message)
		{
			return str.VerifyArgumentMeetsCriteria(a => !string.IsNullOrEmpty(a), message);
		}

		/// <summary>
		/// Throws an ArgumentException if string is null or whitespace
		/// </summary>
		/// <param name="str">String to test</param>
		/// <param name="message">Message to display if string is null or whitespace</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static string VerifyArgumentNotNullOrWhiteSpace(this string str, string message)
		{
			return str.VerifyArgumentMeetsCriteria(a => !string.IsNullOrWhiteSpace(a), message);
		}

		/// <summary>
		/// Throws an ArgumentException if string is not within the correct minimum and/or maximum length or if string is null
		/// </summary>
		/// <param name="str">String to test</param>
		/// <param name="minimum">Minimum length (if null, there is no minimum)</param>
		/// <param name="maximum">Maximum length (if null, there is no maximum)</param>
		/// <param name="message">Message to display if string is not within the correct minimum and/or maximum length or if string is null</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static string VerifyArgumentLength(this string str, int? minimum, int? maximum, string message)
		{
            return str.VerifyArgumentLength(minimum, maximum, false, message);
		}

        /// <summary>
		/// Throws an ArgumentException if string is not within the correct minimum and/or maximum length or if string is null
		/// </summary>
		/// <param name="str">String to test</param>
		/// <param name="minimum">Minimum length (if null, there is no minimum)</param>
		/// <param name="maximum">Maximum length (if null, there is no maximum)</param>
		/// <param name="trimStringFirst">Trim string prior to checking minimum and maximum (default = true)</param>
		/// <param name="message">Message to display if string is not within the correct minimum and/or maximum length or if string is null</param>
		/// <returns></returns>
		[DebuggerStepThrough]
        public static string VerifyArgumentLength(this string str, int? minimum, int? maximum, bool trimStringFirst, string message)
        {
            var localStr = str;

            if (trimStringFirst && str != null)
                localStr = str.Trim();

            Func<string, bool> condition = a => a != null && (!minimum.HasValue || a.Length >= minimum) && (!maximum.HasValue || a.Length <= maximum);
            localStr.VerifyArgumentMeetsCriteria(condition, message);

            return str;
        }

        /// <summary>
		/// Throws an ArgumentException if int is not within the correct range
		/// </summary>
		/// <param name="num">Int to test</param>
		/// <param name="minimum">Minimum value (if null, there is no minimum)</param>
		/// <param name="maximum">Maximum value (if null, there is no maximum)</param>
		/// <param name="message">Message to display if int is not within the correct range</param>
		/// <returns></returns>
		[DebuggerStepThrough]
        public static int VerifyArgumentRange(this int num, int? minimum, int? maximum, string message)
        {
            Func<int, bool> condition = a => (!minimum.HasValue || a >= minimum) && (!maximum.HasValue || a <= maximum);
            num.VerifyArgumentMeetsCriteria(condition, message);

            return num;
        }

        /// <summary>
        /// Throws an ArgumentException if acceptance criteria is not met
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Value to test</param>
        /// <param name="acceptanceCriteria">Criteria for the value to be valid</param>
        /// <param name="message">Message to display if acceptance criteria is not met</param>
        /// <returns></returns>
        [DebuggerStepThrough]
		public static T VerifyArgumentMeetsCriteria<T>(this T obj, Func<T, bool> acceptanceCriteria, string message)
		{
			if (!acceptanceCriteria(obj))
				throw new ArgumentException(message);

			return obj;
		}
    }
}
