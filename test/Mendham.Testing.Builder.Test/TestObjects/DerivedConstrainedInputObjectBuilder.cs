﻿using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.Builder.Test.TestObjects
{
    [MendhamBuilder]
    public class DerivedConstrainedInputObjectBuilder : DataBuilder<DerivedConstrainedInputObject>
    {
        private string value;
        private int derivedValue;

        public DerivedConstrainedInputObjectBuilder()
        {
            this.value = CreateAnonymous<string>()
                .Substring(0, 3);
            this.derivedValue = CreateAnonymous<int>();
        }

        public DerivedConstrainedInputObjectBuilder WithValue(string value)
        {
            this.value = value;
            return this;
        }

        public DerivedConstrainedInputObjectBuilder WithDerivedValue(int derivedValue)
        {
            this.derivedValue = derivedValue;
            return this;
        }

        protected override DerivedConstrainedInputObject BuildObject()
        {
            return new DerivedConstrainedInputObject(value, derivedValue);
        }
    }
}