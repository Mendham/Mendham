using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Mendham.Testing.Helpers;
using Ploeh.AutoFixture;

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

		protected TResult CreateAnonymous<TResult>()
		{
			return SharedFixture.Create<TResult>();
		}

		protected TResult CreateAnonymous<TResult>(TResult seed)
		{
			return SharedFixture.Create<TResult>(seed);
		}

		public static implicit operator T(DataBuilder<T> builder)
		{
			return builder.Build();
		}

		protected static void SetPropertyNonPublicSetter<TSource, TProperty>(TSource obj, Expression<Func<TSource, TProperty>> propertyLambda, object value)
		{
			obj.SetPropertyNonPublicSetter(propertyLambda, value);
		}

		protected static void SetNonPublicMember<TSource>(TSource obj, string memberName, object value)
		{
			obj.SetNonPublicMember(memberName, value);
		}

		protected static void CallNonPublicMethod(T obj, string methodName, params object[] parameters)
		{
			obj.CallNonPublicMethod(methodName, parameters);
		}
	}
}