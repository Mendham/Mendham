using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    /// <summary>
    /// Use this attribute on an IBuilder<T> class to use this builder as the default way to create a type
    /// when using MendhamFixture
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DefaultBuilderAttribute : Attribute
    {
        public DefaultBuilderAttribute()
        { }

        /// <param name="typeOverride"> The type to be applied by the builder if not default T in IBuilder<T>. The T in IBuilder<T> must be 
        /// assignable to the TypeOverride or an exception will occur</param>
        public DefaultBuilderAttribute(Type typeOverride)
        {
            this.TypeOverride = typeOverride;
        }

        /// <summary>
        /// The type to be applied by the builder if not default T in IBuilder<T>. The T in IBuilder<T> must be 
        /// assignable to the TypeOverride or an exception will occur
        /// </summary>
        public Type TypeOverride { get; private set; }
    }
}
