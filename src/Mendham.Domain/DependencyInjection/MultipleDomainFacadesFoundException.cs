using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mendham.Domain.DependencyInjection
{
    public class MultipleDomainFacadesFoundException : Exception
    {
        private readonly Type _interfaceToBind;
        private readonly IEnumerable<Type> _typesImplementingInterface;

        public MultipleDomainFacadesFoundException(Type interfaceToBind, IEnumerable<Type> typesImplementingInterface)
        {
            _interfaceToBind = interfaceToBind
                .VerifyArgumentNotDefaultValue(nameof(interfaceToBind));
            _typesImplementingInterface = typesImplementingInterface
                .VerifyArgumentNotNullOrEmpty(nameof(typesImplementingInterface));
        }

        public Type InterfaceToBind
        {
            get
            {
                return _interfaceToBind;
            }
        }

        public IEnumerable<Type> TypesImplementingInterface
        {
            get
            {
                return _typesImplementingInterface;
            }
        }

        public override string Message
        {
            get
            {
                var concreteTypes = string.Join(", ", TypesImplementingInterface.Select(a => a.FullName));

                var sb = new StringBuilder();
                sb.AppendLine($"When attempting register domain facades, an interface implementing {nameof(IDomainFacade)} was included on multiple classes.");
                sb.AppendLine($"The interface is: {InterfaceToBind.FullName}");
                sb.AppendLine($"The classes that implement this interface are: {concreteTypes}");
                sb.AppendLine($"To resolve the issue, modify code so that {InterfaceToBind.FullName} is only assignable from one concreate class in the assembly.");

                return sb.ToString();
            }
        }
    }
}
