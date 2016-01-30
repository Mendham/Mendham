using System.Reflection;

namespace Mendham.Testing.Builder
{
    interface IParameterInfoCreation
    {
        object Create(ParameterInfo parameterInfo);
        object Create(ParameterInfo parameterInfo, int countForMultiple);
    }
}
