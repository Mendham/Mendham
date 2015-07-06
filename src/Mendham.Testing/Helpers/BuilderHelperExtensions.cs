using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Helpers
{
    public static class BuilderHelperExtensions
    {
		public static void SetPropertyNonPublicSetter<TSource, TProperty>(this TSource obj, Expression<Func<TSource, TProperty>> propertyLambda, object value)
		{
			var propertyInfo = GetPropertyInfo(propertyLambda);
			propertyInfo.SetValue(obj, value);
		}

		public static void SetNonPublicMember<TSource>(this TSource obj, string memberName, object value)
		{
			Type type = typeof(TSource);

			type.GetField(memberName, BindingFlags.NonPublic | BindingFlags.Instance)
				.SetValue(obj, value);
		}

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
