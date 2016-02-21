using System;

namespace Mendham.Domain.DependencyInjection.Autofac
{
    public class InvalidDomainFacadeExclusionException : Exception
    {
        private readonly Type _invalidType;

        public InvalidDomainFacadeExclusionException(Type invalidType)
        {
            _invalidType = invalidType
                .VerifyArgumentNotDefaultValue(nameof(invalidType));
        }

        public Type InvalidType { get { return _invalidType; } }

        public override string Message
        {
            get
            {
                return $"When attempting to register domain facades, the exclusion list contained an invalid type {_invalidType.FullName}. "
                    + $"The exclusion list can only include types that are interfaces implementing {typeof(IDomainFacade).FullName}.";
            }
        }
    }
}
