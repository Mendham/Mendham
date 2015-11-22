using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Testing.Builder.Test
{
    public class BuilderAttributeResolverTest
    {
        [Fact]
        public void GetAttributesAppliedToClass_SingleAttribute_OneResult()
        {
            var sut = new BuilderAttributeResolver();

            var result = sut.GetAttributesAppliedToClass<TestAttribute>(typeof(ClassWithAttributeA));

            result.Should()
                .HaveCount(1)
                .And.OnlyContain(a => a.Value == 5);
        }

        [Fact]
        public void GetAttributesAppliedToClass_MultipleAttributes_ThreeResults()
        {
            var sut = new BuilderAttributeResolver();

            var result = sut.GetAttributesAppliedToClass<TestAttribute>(typeof(ClassWithAttributeB));

            result.Should()
                .HaveCount(3)
                .And.Contain(a => a.Value == 4)
                .And.Contain(a => a.Value == 9)
                .And.Contain(a => a.Value == 12);
        }

        #region TestObjects
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class TestAttribute : Attribute
        {
            public TestAttribute(int value)
            {
                this.Value = value;
            }

            public int Value { get; private set; }
        }

        [Test(5)]
        public class ClassWithAttributeA
        { }

        [Test(4)]
        [Test(9)]
        [Test(12)]
        public class ClassWithAttributeB
        { } 
        #endregion
    }
}
