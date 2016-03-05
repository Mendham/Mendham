using Mendham.Testing.Builder;
using Mendham.Testing.Helpers;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Mendham.Testing
{
    /// <summary>
    /// Base class for a <see cref="IBuilder{T}"/>
    /// </summary>
    /// <typeparam name="T">Object to build</typeparam>
	public abstract class Builder<T> : IBuilder<T>
    {
        private IObjectCreationContext _objectCreationContext;

        [DebuggerStepThrough]
        public Builder()
        { }

        /// <summary>
        /// Build an object of <typeparamref name="T"/>
        /// </summary>
        /// <returns></returns>
		[DebuggerStepThrough()]
		public T Build()
		{
			T obj = BuildObject();
			return obj;
		}

        /// <summary>
        /// Protected method within builder that is responsible for constructing the object of type <typeparamref name="T"/>
        /// </summary>
        /// <returns>Object of type <typeparamref name="T"/></returns>
        protected abstract T BuildObject();

        /// <summary>
        /// The <see cref="IObjectCreationContext"/> used for building new objects for the builder.
        /// </summary>
        protected IObjectCreationContext ObjectCreationContext
        {
            get
            {
                if (_objectCreationContext == null)
                {
                    _objectCreationContext = ObjectCreationContextFactory.CreateObjectCreationContext(this.GetType().Assembly);
                }

                return _objectCreationContext;
            }
        }

        /// <summary>
        /// Handles implicit conversion of a <see cref="Builder{T}"/> to an object of type <typeparamref name="{T}"/>
        /// </summary>
        /// <param name="builder"></param>
        public static implicit operator T(Builder<T> builder)
		{
			return builder.Build();
		}

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
		protected static void SetPropertyNonPublicSetter<TSource, TProperty>(TSource obj, Expression<Func<TSource, TProperty>> propertyLambda, TProperty value)
		{
			obj.SetPropertyNonPublicSetter(propertyLambda, value);
		}

        /// <summary>
        /// Applies <paramref name="value"/> to non public member with name defined in <paramref name="memberName"/> 
        /// in object <paramref name="obj"/>
        /// </summary>
        /// <typeparam name="TSource">Type of object with non public member</typeparam>
        /// <param name="obj">Object with non public member</param>
        /// <param name="memberName">String of name of non public member</param>
        /// <param name="value">Value to be applied to the property with an non public member</param>
		protected static void SetNonPublicMember<TSource>(TSource obj, string memberName, object value)
		{
			obj.SetNonPublicMember(memberName, value);
		}

        /// <summary>
        /// Calls non public method with name of <paramref name="methodName"/> in object <paramref name="obj"/> with 
        /// parameters specified in <paramref name="parameters"/>
        /// </summary>
        /// <param name="obj">Object with non public method</param>
        /// <param name="methodName">Name of method</param>
        /// <param name="parameters">Parameters to be passed to method when called</param>
		protected static void CallNonPublicMethod(T obj, string methodName, params object[] parameters)
		{
			obj.CallNonPublicMethod(methodName, parameters);
		}
	}

    /// <summary>
    /// Base class for <see cref="IBuilder{T}"/>. This version of the builder has additional support to get a builder 
    /// factory that can be used to build collections of the object
    /// </summary>
    /// <typeparam name="TObject">Object to build</typeparam>
    /// <typeparam name="TBuilder">
    /// The builder being used. (This value should be the class deriving from <see cref="Builder{TObject, TBuilder}"/>)
    /// </typeparam>
    public abstract class Builder<TObject, TBuilder> : Builder<TObject>
        where TBuilder : IBuilder<TObject>, new()
    {
        /// <summary>
        /// Gets default factory to produce <typeparamref name="TBuilder"/>. The result can be used to produce multiple 
        /// unqiue builders which can be used to create a collection of <typeparamref name="TObject"/>
        /// </summary>
        /// <returns>A <see cref="Func{IBuilder{TObject}}"/> used to create a <typeparamref name="TObject"/></returns>
        public static Func<IBuilder<TObject>> GetFactory()
        {
            return () => new TBuilder();
        }

        /// <summary>
        /// Gets a factory to produce <typeparamref name="TBuilder"/>. The result of the factory can be modified by 
        /// value in <paramref name="builderSetup"/>. The result can be used to produce multiple unqiue builders which
        /// can be used to create a collection of <typeparamref name="TObject"/>
        /// Gets factory to produce a <see cref="IBuilder{TObject}"/> which is used for producing multiple builders
        /// which is needed when trying to create a collection of <typeparamref name="TObject"/>
        /// </summary>
        /// <param name="builderSetup">A builder to create a <typeparamref name="TObject"/></param>
        /// <returns>A <see cref="Func{IBuilder{TObject}}"/> used to create a <typeparamref name="TObject"/></returns>
        public static Func<IBuilder<TObject>> GetFactory(Func<TBuilder, TBuilder> builderSetup)
        {
            return () => builderSetup(new TBuilder());
        }
    }
}