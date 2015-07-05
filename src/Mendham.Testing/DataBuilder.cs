using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Mendham.Testing
{
	public abstract class DataBuilder<T> : IBuilder<T>
	{
		private static Ploeh.AutoFixture.Fixture _sharedFixture;
		private static Ploeh.AutoFixture.Fixture SharedFixture
		{
			get
			{
				return _sharedFixture = _sharedFixture ?? new Ploeh.AutoFixture.Fixture();
			}
		}

		protected Ploeh.AutoFixture.Fixture AutoFixture { get; private set; }

		[DebuggerStepThrough]
		public DataBuilder()
		{
			this.AutoFixture = SharedFixture;
		}

		[DebuggerStepThrough()]
		public T Build()
		{
			var o = BuildObject();
			return o;
		}

		protected abstract T BuildObject();

		public static implicit operator T(DataBuilder<T> builder)
		{
			return builder.Build();
		}

		protected static void SetPropertyNonPublicSetter<TSource, TProperty>(TSource obj, Expression<Func<TSource, TProperty>> propertyLambda, object value)
		{
			var propertyInfo = GetPropertyInfo(propertyLambda);
			propertyInfo.SetValue(obj, value);
		}

		protected static void SetNonPublicMember<TSource>(TSource obj, string memberName, object value)
		{
			Type type = typeof(TSource);

			type.GetField(memberName, BindingFlags.NonPublic | BindingFlags.Instance)
				.SetValue(obj, value);
		}

		private static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
		{
			Type type = typeof(TSource);

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

			if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
				throw new ArgumentException(string.Format(
					"Expresion '{0}' refers to a property that is not from type {1}.",
					propertyLambda.ToString(), type));

			return propInfo;
		}

		protected static void CallNonPublicMethod(T obj, string methodName, params object[] parameters)
		{
			BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
			MethodInfo methodInfo = typeof(T).GetMethod(methodName, flags);

			methodInfo.Invoke(obj, parameters);
		}
	}
}
