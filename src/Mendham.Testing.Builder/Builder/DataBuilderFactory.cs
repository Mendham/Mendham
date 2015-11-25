using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public class DataBuilderFactory
    {
        public Type BuilderType { get; private set; }

        public DataBuilderFactory(Type builderType)
        {
            builderType.VerifyArgumentNotDefaultValue("Builder Type is required and cannot default(type)")
                .VerifyArgumentMeetsCriteria(a => a.ImplementsIBuilder(), "Type passed to data factory is not of type IBuilder<>");

            this.BuilderType = builderType;
        }

        public bool IsBuilderMatch(Type otherBuilderType)
        {
            return this.BuilderType.Equals(otherBuilderType);
        }

        private object BuildObject()
        {
            var builder = Activator.CreateInstance(BuilderType);
            MethodInfo method = BuilderType.GetMethod("Build");
            return method.Invoke(builder, null);
        }

        public T Build<T>()
        {
            return (T)Build(typeof(T));
        }

        public object Build(Type typeToBuild)
        {
            var obj = BuildObject();

            if (!typeToBuild.IsAssignableFrom(obj.GetType()))
            {
                var msg = string.Format(
                    CultureInfo.CurrentCulture,
                    "The type built {0} was not assignable to the type request {1}. Check DataTheoryBuilderAttribute configuration",
                    obj.GetType().FullName,
                    typeToBuild.FullName);

                throw new InvalidOperationException(msg);
            }

            return obj;
        }
    }
}
