using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham
{
	public static class ArgumentVerificationExtensions
	{
		/// <summary>
		/// Throws an ArgumentException if value is null
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">Value to test</param>
		/// <param name="message">Message to display if value is null</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static T VerifyArgumentNotNull<T>(this T obj, String message)
		{
			return obj.VerifyArgumentMeetsCriteria(a => a != null, message);
		}

		/// <summary>
		/// Throws an ArgumentException if object is null or default value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="tObj">Value to test</param>
		/// <param name="message">Message to display if value is null or default value</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static T VerifyArgumentNotDefaultValue<T>(this T tObj, String message)
		{
			return tObj.VerifyArgumentMeetsCriteria(a => a != null && !a.Equals(default(T)), message);
		}

		/// <summary>
		/// Throws an ArgumentException if enumerable is null or empty
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="tEnumerable">Enumerable to test</param>
		/// <param name="message">Message to display if enumerable is null or empty</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static IEnumerable<T> VerifyArgumentNotNullOrEmpty<T>(this IEnumerable<T> tEnumerable, String message)
		{
			return tEnumerable.VerifyArgumentMeetsCriteria(a => a != null && tEnumerable.Any(), message);
		}

		/// <summary>
		/// Throws an ArgumentException if string is null or empty
		/// </summary>
		/// <param name="str">String to test</param>
		/// <param name="message">Message to display if string is null or empty</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static String VerifyArgumentNotNullOrEmpty(this String str, String message)
		{
			return str.VerifyArgumentMeetsCriteria(a => !String.IsNullOrEmpty(a), message);
		}

		/// <summary>
		/// Throws an ArgumentException if string is null or whitespace
		/// </summary>
		/// <param name="str">String to test</param>
		/// <param name="message">Message to display if string is null or whitespace</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static String VerifyArgumentNotNullOrWhiteSpace(this String str, String message)
		{
			return str.VerifyArgumentMeetsCriteria(a => !String.IsNullOrWhiteSpace(a), message);
		}

		/// <summary>
		/// Throws an ArgumentException if string is not within the correct minium and/or maxium length or if string is null
		/// </summary>
		/// <param name="str">String to test</param>
		/// <param name="minimum">Minimum length (if null, there is no minium)</param>
		/// <param name="maximum">Maxium length (if null, there is no maximum)</param>
		/// <param name="message">Message to display if string is not within the correct minium and/or maxium length or if string is null</param>
		/// <param name="trimStringFirst">(Optional) Trim string prior to checking minium and maximum (default = true)</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static String VerifyArgumentLength(this String str, int? minimum, int? maximum, String message, bool trimStringFirst = true)
		{
			var localStr = str;

			if (trimStringFirst && str != null)
				localStr = str.Trim();

			Func<String, bool> condition = a => a != null && (!minimum.HasValue || a.Length >= minimum) && (!maximum.HasValue || a.Length <= maximum);
			localStr.VerifyArgumentMeetsCriteria(condition, message);

			return str;
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
		public static T VerifyArgumentMeetsCriteria<T>(this T obj, Func<T, bool> acceptanceCriteria, String message)
		{
			if (!acceptanceCriteria(obj))
				throw new ArgumentException(message);

			return obj;
		}
	}
}
