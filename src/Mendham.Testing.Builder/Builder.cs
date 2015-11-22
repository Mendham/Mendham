using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Mendham.Testing.Helpers;
using Ploeh.AutoFixture;
using Mendham.Testing.Builder;

namespace Mendham.Testing
{
	public abstract class DataBuilder<T> : IBuilder<T>
    {
		private static IObjectCreationContext _objectCreationContext;
		private static IObjectCreationContext ObjectCreationContext
		{
			get
			{
                if (_objectCreationContext == default(IObjectCreationContext))
                {
                    _objectCreationContext = new ObjectCreationContext(typeof(DataBuilder<T>).Assembly);
                }

                return _objectCreationContext;
			}
		}

		[DebuggerStepThrough]
		public DataBuilder()
		{ }

		[DebuggerStepThrough()]
		public T Build()
		{
			var o = BuildObject();
			return o;
		}

		protected abstract T BuildObject();

		protected TResult CreateAnonymous<TResult>()
		{
			return ObjectCreationContext.Create<TResult>();
		}

		protected TResult CreateAnonymous<TResult>(TResult seed)
		{
			return ObjectCreationContext.Create<TResult>(seed);
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