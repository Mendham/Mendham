﻿using Mendham.Testing.Builder.Exceptions;
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
        private readonly ConcurrentDictionary<Type, DataBuilderFactory> registeredTypes;
        private readonly IBuilderQueryService builderQuerySvc;
        private readonly IBuilderAttributeResolver builderAttributeResolver;

        private bool isComplete = false;


        public BuilderRegistration(IBuilderQueryService builderQuerySvc, IBuilderAttributeResolver builderAttributeResolver)
        {
            builderQuerySvc.VerifyArgumentNotDefaultValue("IBuilderQueryService is required");
            builderAttributeResolver.VerifyArgumentNotDefaultValue("Builder Attribute Resolver is required");

            this.builderQuerySvc = builderQuerySvc;
            this.builderAttributeResolver = builderAttributeResolver;

            registeredTypes = new ConcurrentDictionary<Type, DataBuilderFactory>();
            isComplete = false;
        }

        public void Register(Assembly assembly)
        {
            if (isComplete)
                return;

            var builderTypes = builderQuerySvc
                .GetBuilderTypes(assembly)
                .Select(ValidateBuilder);

            foreach (var builderType in builderTypes)
            {
                if (isComplete)
                    return;

                AddBuilderTypesToRegistation(builderType, builderAttributeResolver);
            }

            isComplete = true;
        }

        /// <summary>
        /// Determines if the exact type T has a builder that is registered
        /// </summary>
        /// <typeparam name="T">Type to be built by builder</typeparam>
        /// <returns>True if registered, false if not</returns>
        public bool IsTypeRegistered<T>()
        {
            if (!isComplete)
                throw new BuilderRegistrationNotRegisteredException();

            return registeredTypes.ContainsKey(typeof(T));
        }

        public T Build<T>()
        {
            if (!isComplete)
                throw new BuilderRegistrationNotRegisteredException();

            DataBuilderFactory mdbFactory = null;

            if (!registeredTypes.TryGetValue(typeof(T), out mdbFactory))
            {
                throw new UnregisteredBuilderTypeException(typeof(T));
            }

            return mdbFactory.Build<T>();
        }

        public bool TryBuild<T>(out T value)
        {
            if (!isComplete)
                throw new BuilderRegistrationNotRegisteredException();

            DataBuilderFactory mdbFactory = null;

            if (!registeredTypes.TryGetValue(typeof(T), out mdbFactory))
            {
                value = default(T);
                return false;
            }

            value = mdbFactory.Build<T>();
            return true;
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
                if (isComplete)
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
                if (!builderAttribute.TypeOverride.IsAssignableFrom(defaultTypeToBeBuilt))
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
            var builderFactory = new DataBuilderFactory(builderType);

            registeredTypes
                .AddOrUpdate(typeToBuild, builderFactory, (key, existingValue) =>
                {
                    if (!existingValue.IsBuilderMatch(builderType))
                    {
                        throw new MultipleBuilderForTypeException(existingValue.BuilderType, 
                            builderType,  typeToBuild);
                    }

                    return existingValue;
                });
        }
    }
}
