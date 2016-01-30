using Mendham.Testing.Builder;
using Mendham.Testing.Helpers;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mendham.Testing
{
    /// <summary>
    /// Base class for a builder
    /// </summary>
    /// <typeparam name="T">Object to build</typeparam>
	public abstract class Builder<T> : IBuilder<T>
    {
		private static IObjectCreationContext _objectCreationContext;
		protected static IObjectCreationContext ObjectCreationContext
		{
			get
			{
                if (_objectCreationContext == default(IObjectCreationContext))
                {
                    _objectCreationContext = ObjectCreationContextFactory.Create(typeof(Builder<T>).Assembly);
                }

                return _objectCreationContext;
			}
		}

		[DebuggerStepThrough]
		public Builder()
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
			return ObjectCreationContext.Create(seed);
		}

		public static implicit operator T(Builder<T> builder)
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

    /// <summary>
    /// Base class for a builder. This version of the builder has additional support to get a builder factory
    /// that can be used to build collections of the object
    /// </summary>
    /// <typeparam name="TObject">Object to build</typeparam>
    /// <typeparam name="TBuilder">The builder being used (should be the same as the derived class)</typeparam>
    public abstract class Builder<TObject, TBuilder> : Builder<TObject>
        where TBuilder : IBuilder<TObject>, new()
    {
        public static Func<IBuilder<TObject>> GetFactory(Func<TBuilder, TBuilder> builder)
        {
            return () => builder(new TBuilder());
        }
    }
}