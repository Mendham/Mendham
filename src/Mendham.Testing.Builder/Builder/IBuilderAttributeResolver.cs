﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder
{
    public interface IBuilderAttributeResolver
    {
        /// <summary>
        /// Gets MendhamBuilderAttribute(s) applied to BuilderType
        /// </summary>
        /// <param name="builderType">Type that implements IBuilder<T></param>
        /// <returns>All MendhamBuilderAttributes applied to BuilderType</returns>
        IEnumerable<MendhamBuilderAttribute> GetAttributesAppliedToBuilder(Type builderType);
    }
}
