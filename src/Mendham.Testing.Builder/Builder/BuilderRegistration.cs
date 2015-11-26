using Mendham.Testing.Builder.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public class BuilderRegistration : IBuilderRegistration
    {
        private readonly Dictionary<Type, DataBuilderFactory> registeredTypes;
        private readonly IBuilderQueryService builderQuerySvc;
        private readonly IBuilderAttributeResolver builderAttributeResolver;

        private bool isRegistered = false;


        public BuilderRegistration(IBuilderQueryService builderQuerySvc, IBuilderAttributeResolver builderAttributeResolver)
        {
            builderQuerySvc.VerifyArgumentNotDefaultValue("IBuilderQueryService is required");
            builderAttributeResolver.VerifyArgumentNotDefaultValue("Builder Attribute Resolver is required");

            this.builderQuerySvc = builderQuerySvc;
            this.builderAttributeResolver = builderAttributeResolver;

            registeredTypes = new Dictionary<Type, DataBuilderFactory>();
            isRegistered = false;
        }

        public void Register(Assembly callingAssembly)
        {
            if (isRegistered)
                return;

            var builderTypes = builderQuerySvc
                .GetBuilderTypes(callingAssembly)
                .Select(ValidateBuilder);

            foreach (var builderType in builderTypes)
            {
                AddBuilderTypesToRegistation(builderType, builderAttributeResolver);
            }

            isRegistered = true;
        }

        /// <summary>
        /// Determines if the exact type T has a builder that is registered
        /// </summary>
        /// <typeparam name="T">Type to be built by builder</typeparam>
        /// <returns>True if registered, false if not</returns>
        public bool IsTypeRegistered<T>()
        {
            return IsTypeRegistered(typeof(T));
        }

        public bool IsTypeRegistered(Type typeToBuild)
        {
            if (!isRegistered)
                throw new BuilderRegistrationNotRegisteredException();

            return registeredTypes.ContainsKey(typeToBuild);
        }

        public T Build<T>()
        {
            if (!isRegistered)
                throw new BuilderRegistrationNotRegisteredException();

            var typeToBuild = typeof(T);
                 
            if (!registeredTypes.ContainsKey(typeToBuild))
                throw new UnregisteredBuilderTypeException(typeToBuild);

            return registeredTypes[typeToBuild].Build<T>();
        }

        public object Build(Type typeToBuild)
        {
            if (!isRegistered)
                throw new BuilderRegistrationNotRegisteredException();

            if (!registeredTypes.ContainsKey(typeToBuild))
                throw new UnregisteredBuilderTypeException(typeToBuild);

            return registeredTypes[typeToBuild].Build(typeToBuild);
        }

        private Type ValidateBuilder(Type builderType)
        {
            var isValid = builderType
                .ImplementsIBuilder();

            if (!isValid)
            {
                throw new InvalidBuilderException(builderType);
            }

            return builderType;
        }

        private void AddBuilderTypesToRegistation(Type builderType, IBuilderAttributeResolver builderAttributeResolver)
        {
            var typesToBuild = builderAttributeResolver
                .GetAttributesAppliedToBuilder(builderType)
                .Select(attribute => GetTypeBuiltByBuilder(builderType, attribute));

            foreach (var typeToBuild in typesToBuild)
            {
                if (isRegistered)
                    return;

                AddToDictionary(builderType, typeToBuild);
            }
        }

        private static Type GetTypeBuiltByBuilder(Type builderType, MendhamBuilderAttribute builderAttribute)
        {
            var defaultTypeToBeBuilt = builderType
                .GetTypeIBuilderBuilds();

            if (builderAttribute.TypeOverride != default(Type))
            {
                if (!builderAttribute.TypeOverride.GetTypeInfo().IsAssignableFrom(defaultTypeToBeBuilt))
                {
                    throw new InvalidMendhamBuilderOverrideException(builderType,
                        builderAttribute.TypeOverride, defaultTypeToBeBuilt);
                }

                return builderAttribute.TypeOverride;
            }

            return defaultTypeToBeBuilt;
        }

        private void AddToDictionary(Type builderType, Type typeToBuild)
        {
            if (registeredTypes.ContainsKey(typeToBuild))
            {
                var existingBuilderType = registeredTypes[typeToBuild].BuilderType;

                if (existingBuilderType != builderType)
                {
                    throw new MultipleBuilderForTypeException(existingBuilderType,
                        builderType, typeToBuild);
                }
                else
                {
                    return;
                }
            }

            var builderFactory = new DataBuilderFactory(builderType);
            registeredTypes[typeToBuild] = builderFactory;
        }
    }
}
