using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Mendham.Testing.Helpers
{
    public static class BuilderHelperExtensions
    {
        /// <summary>
        /// Applies <paramref name="value"/> to a property in <paramref name="obj"/> that has a non public setter
        /// </summary>
        /// <typeparam name="TSource">Type of object</typeparam>
        /// <typeparam name="TProperty">Type of property being applied to <paramref name="obj"/></typeparam>
        /// <param name="obj">Object with property that has non public setter</param>
        /// <param name="propertyLambda">
        /// Expression that represents property in <paramref name="obj"/> that has a non public setter
        /// </param>
        /// <param name="value">Value to be applied to property with non public setter</param>
		public static void SetPropertyNonPublicSetter<TSource, TProperty>(this TSource obj, Expression<Func<TSource, TProperty>> propertyLambda, object value)
		{
			var propertyInfo = GetPropertyInfo(propertyLambda);
			propertyInfo.SetValue(obj, value);
		}

        /// <summary>
        /// Applies <paramref name="value"/> to non public member with name defined in <paramref name="memberName"/> 
        /// in object <paramref name="obj"/>
        /// </summary>
        /// <typeparam name="TSource">Type of object with non public member</typeparam>
        /// <param name="obj">Object with non public member</param>
        /// <param name="memberName">String of name of non public member</param>
        /// <param name="value">Value to be applied to the property with an non public member</param>
		public static void SetNonPublicMember<TSource>(this TSource obj, string memberName, object value)
		{
			Type type = typeof(TSource);

			type.GetField(memberName, BindingFlags.NonPublic | BindingFlags.Instance)
				.SetValue(obj, value);
		}

        /// <summary>
        /// Calls non public method with name of <paramref name="methodName"/> in object <paramref name="obj"/> with 
        /// parameters specified in <paramref name="parameters"/>
        /// </summary>
        /// <param name="obj">Object with non public method</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters to be passed to method when called</param>
		public static void CallNonPublicMethod<T>(this T obj, string methodName, params object[] parameters)
		{
			BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
			MethodInfo methodInfo = typeof(T).GetMethod(methodName, flags);

			methodInfo.Invoke(obj, parameters);
		}

		private static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
		{
			MemberExpression member = propertyLambda.Body as MemberExpression;
			if (member == null)
				throw new ArgumentException(string.Format(
					"Expression '{0}' refers to a method, not a property.",
					propertyLambda.ToString()));

			PropertyInfo propInfo = member.Member as PropertyInfo;
			if (propInfo == null)
				throw new ArgumentException(string.Format(
					"Expression '{0}' refers to a field, not a property.",
					propertyLambda.ToString()));

			Type type = typeof(TSource);
			var typeInfo = type.GetTypeInfo();

			if (type != propInfo.DeclaringType && !typeInfo.IsSubclassOf(propInfo.DeclaringType))
				throw new ArgumentException(string.Format(
					"Expresion '{0}' refers to a property that is not from type {1}.",
					propertyLambda.ToString(), type));

			return propInfo;
		}
	}
}
