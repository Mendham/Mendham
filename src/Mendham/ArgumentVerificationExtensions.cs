using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
		/// Throws an ArgumentException if value is null
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">Value to test</param>
        /// <param name="customException">Exception to throw when verification fails</param>
		/// <returns></returns>
		[DebuggerStepThrough]
        public static T VerifyArgumentNotNull<T>(this T obj, Func<Exception> customException)
        {
            if (obj != null)
            {
                return obj;
            }
            else
            {
                throw customException();
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
        /// Throws an ArgumentException if object is null or default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tObj">Value to test</param>
        /// <param name="customException">Exception to throw when verification fails</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T VerifyArgumentNotDefaultValue<T>(this T tObj, Func<Exception> customException)
        {
            if (!Equals(tObj, default(T)))
            {
                return tObj;
            }

            throw customException();
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
        /// Throws an ArgumentException if enumerable is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tEnumerable">Enumerable to test</param>
        /// <param name="customException">Exception to throw when verification fails</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<T> VerifyArgumentNotNullOrEmpty<T>(this IEnumerable<T> tEnumerable, Func<Exception> customException)
        {
            tEnumerable.VerifyArgumentNotNull(customException);

            if (tEnumerable.Any())
            {
                return tEnumerable;
            }
            else 
            {
                throw customException();
            }
        }

        /// <summary>
        /// Throws an ArgumentException if string is null or empty
        /// </summary>
        /// <param name="str">String to test</param>
        /// <param name="paramName">Name of parameter</param>
        /// <param name="message">Message to display if string is null or empty (optional)</param>
        /// <returns></returns>
        [DebuggerStepThrough]
		public static string VerifyArgumentNotNullOrEmpty(this string str, string paramName, string message = null)
		{
            str.VerifyArgumentNotNull(paramName, message);

            if (!string.IsNullOrEmpty(str))
            {
                return str;
            }
            else if (message == null)
            {
                message = $"Parameter '{paramName}' cannot be an empty string.";
            }

            throw new ArgumentException(message, paramName);
		}

        /// <summary>
        /// Throws an ArgumentException if string is null or empty
        /// </summary>
        /// <param name="str">String to test</param>
        /// <param name="customException">Exception to throw when verification fails</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string VerifyArgumentNotNullOrEmpty(this string str, Func<Exception> customException)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw customException();
            }

            return str;
        }

        /// <summary>
        /// Throws an ArgumentException if string is null or white-space
        /// </summary>
        /// <param name="str">String to test</param>
        /// <param name="paramName">Name of parameter</param>
        /// <param name="message">Message to display if string is null or white-space (optional)</param>
        /// <returns></returns>
        [DebuggerStepThrough]
		public static string VerifyArgumentNotNullOrWhiteSpace(this string str, string paramName, string message = null)
		{
            str.VerifyArgumentNotNull(paramName, message);

            if (!string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            else if (message == null)
            {
                message = $"Parameter '{paramName}' cannot be an empty string or white-space.";
            }

            throw new ArgumentException(message, paramName);
		}

        /// <summary>
        /// Throws an ArgumentException if string is null or white-space
        /// </summary>
        /// <param name="str">String to test</param>
        /// <param name="customException">Exception to throw when verification fails</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string VerifyArgumentNotNullOrWhiteSpace(this string str, Func<Exception> customException)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw customException();
            }

            return str;
        }

        /// <summary>
        /// Throws an ArgumentException if string is not within the correct minimum and/or maximum length or if string is null
        /// </summary>
        /// <param name="str">String to test</param>
        /// <param name="minimum">Minimum length (if null, there is no minimum)</param>
        /// <param name="maximum">Maximum length (if null, there is no maximum)</param>
        /// <param name="paramName">Name of parameter</param>
        /// <param name="message">Message to display if string is not within the correct minimum and/or maximum length or if string is null</param>
        /// <returns></returns>
        [DebuggerStepThrough]
		public static string VerifyArgumentLength(this string str, int? minimum, int? maximum, string paramName, string message = null)
        {
            return str.VerifyArgumentLength(minimum, maximum, false, paramName, message);
		}

        /// <summary>
        /// Throws an ArgumentException if string is not within the correct minimum and/or maximum length or if string is null
        /// </summary>
        /// <param name="str">String to test</param>
        /// <param name="minimum">Minimum length (if null, there is no minimum)</param>
        /// <param name="maximum">Maximum length (if null, there is no maximum)</param>
        /// <param name="customException">Exception to throw when verification fails</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string VerifyArgumentLength(this string str, int? minimum, int? maximum, Func<Exception> customException)
        {
            return str.VerifyArgumentLength(minimum, maximum, false, customException);
        }

        /// <summary>
		/// Throws an ArgumentException if string is not within the correct minimum and/or maximum length or if string is null
		/// </summary>
		/// <param name="str">String to test</param>
        /// <param name="minimum">Minimum length (if null, there is no minimum)</param>
		/// <param name="maximum">Maximum length (if null, there is no maximum)</param>
		/// <param name="trimStringFirst">Trim string prior to checking minimum and maximum (default = true)</param>
		/// <param name="paramName">Name of parameter</param>
		/// <param name="message">Message to display if string is not within the correct minimum and/or maximum length or if string is null</param>
		/// <returns></returns>
		[DebuggerStepThrough]
        public static string VerifyArgumentLength(this string str, int? minimum, int? maximum, bool trimStringFirst, string paramName, string message = null)
        {
            Func<Exception> exBuilder = () =>
            {
                var msg = BuildStringArgumentLengthMessage(str, minimum, maximum, trimStringFirst, paramName, message);
                throw new ArgumentException(msg, paramName);
            };

            return str.VerifyArgumentNotNull(paramName, message)
                .VerifyArgumentLength(minimum, maximum, trimStringFirst, exBuilder);
        }

        /// <summary>
        /// Throws an ArgumentException if string is not within the correct minimum and/or maximum length or if string is null
        /// </summary>
        /// <param name="str">String to test</param>
        /// <param name="minimum">Minimum length (if null, there is no minimum)</param>
        /// <param name="maximum">Maximum length (if null, there is no maximum)</param>
        /// <param name="trimStringFirst">Trim string prior to checking minimum and maximum (default = true)</param>
        /// <param name="customException">Exception to throw when verification fails</param>length or if string is null</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string VerifyArgumentLength(this string str, int? minimum, int? maximum, bool trimStringFirst, Func<Exception> customException)
        {
            var localStr = str.VerifyArgumentNotNull(customException);

            if (trimStringFirst && str != null)
            {
                localStr = str.Trim();
            }

            if ((!minimum.HasValue || localStr.Length >= minimum) && (!maximum.HasValue || localStr.Length <= maximum))
            {
                return str;
            }

            throw customException();
        }

        private static string BuildStringArgumentLengthMessage(string str, int? minimum, int? maximum, bool trimStringFirst, string paramName, string message)
        {
            var msgSb = new StringBuilder("The length of the string was not within the permitted range.");
            msgSb.AppendLine();
            msgSb.AppendLine($"String value: '{str}'");

            if (trimStringFirst)
            {
                str = str.Trim();
            }

            msgSb.AppendLine($"Actual length: {str.Length}");
            
            if (minimum.HasValue)
            {
                msgSb.AppendLine($"Minimum valid length: {minimum.Value}");
            }
            if (maximum.HasValue)
            {
                msgSb.AppendLine($"Maximum valid length: {maximum.Value}");
            }
            if (trimStringFirst)
            {
                msgSb.AppendLine("Leading or trailing white-space is not considered when determining the length of the string");
            }
            if (!string.IsNullOrWhiteSpace(message))
            {
                msgSb.AppendLine();
                msgSb.AppendLine($"Additional Information: {message}");
            }

            return msgSb.ToString();
        }

        /// <summary>
		/// Throws an ArgumentException if int is not within the correct range
		/// </summary>
		/// <param name="num">Int to test</param>
        /// <param name="minimum">Minimum value (if null, there is no minimum)</param>
		/// <param name="maximum">Maximum value (if null, there is no maximum)</param>
		/// <param name="paramName">Name of parameter</param>
		/// <param name="message">Message to display if int is not within the correct range (optional)</param>
		/// <returns></returns>
		[DebuggerStepThrough]
        public static int VerifyArgumentRange(this int num, int? minimum, int? maximum, string paramName, string message = null)
        {
            Func<ArgumentOutOfRangeException> exBuilder = () =>
            {
                var msgSb = new StringBuilder("The value is not within the permitted range");
                msgSb.AppendLine();
                msgSb.AppendLine($"Value: '{num}'");

                if (minimum.HasValue)
                {
                    msgSb.AppendLine($"Minimum value: {minimum.Value}");
                }
                if (maximum.HasValue)
                {
                    msgSb.AppendLine($"Maximum value: {maximum.Value}");
                }
                if (!string.IsNullOrWhiteSpace(message))
                {
                    msgSb.AppendLine();
                    msgSb.AppendLine($"ADDITIONAL INFORMATION: {message}");
                }

                throw new ArgumentOutOfRangeException(paramName, num, msgSb.ToString());
            };

            return num.VerifyArgumentRange(minimum, maximum, exBuilder);
        }

        /// <summary>
        /// Throws an ArgumentException if int is not within the correct range
        /// </summary>
        /// <param name="num">Int to test</param>
        /// <param name="minimum">Minimum value (if null, there is no minimum)</param>
        /// <param name="maximum">Maximum value (if null, there is no maximum)</param>
        /// <param name="customException">Exception to throw when verification fails</param>length or if string is null</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static int VerifyArgumentRange(this int num, int? minimum, int? maximum, Func<Exception> customException)
        {
            if ((!minimum.HasValue || num >= minimum) && (!maximum.HasValue || num <= maximum))
            {
                return num;
            }

            throw customException();
        }

        /// <summary>
        /// Throws an ArgumentException if acceptance criteria is not met
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Value to test</param>
        /// <param name="acceptanceCriteria">Criteria for the value to be valid</param>
        /// <param name="paramName">Name of parameter</param>
        /// <param name="message">Message to display if acceptance criteria is not met</param>
        /// <returns></returns>
        [DebuggerStepThrough]
		public static T VerifyArgumentMeetsCriteria<T>(this T obj, Func<T, bool> acceptanceCriteria, string paramName, string message)
        {
            return obj.VerifyArgumentMeetsCriteria(acceptanceCriteria, () => new ArgumentException(message, paramName));
        }

        /// <summary>
        /// Throws an ArgumentException if acceptance criteria is not met
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Value to test</param>
        /// <param name="acceptanceCriteria">Criteria for the value to be valid</param>
        /// <param name="customException">Exception to throw when verification fails</param>length or if string is null</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T VerifyArgumentMeetsCriteria<T>(this T obj, Func<T, bool> acceptanceCriteria, Func<Exception> customException)
        {
            if (!acceptanceCriteria(obj))
            {
                throw customException();
            }

            return obj;
        }
    }
}
