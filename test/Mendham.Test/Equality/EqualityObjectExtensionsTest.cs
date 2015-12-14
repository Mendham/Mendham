using FluentAssertions;
using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mendham.Test.Equality
{
    public class EqualityObjectExtensionsTest
    {
        [Fact]
        public void IsObjectSameType_SameReference_True()
        {
            object o = new object();

            var result = o.IsObjectSameType(o);

            result.Should()
                .BeTrue("object is the same reference");
        }

        [Fact]
        public void IsObjectSameType_SameObjectType_True()
        {
            object o1 = new object();
            object o2 = new object();

            var result = o1.IsObjectSameType(o2);

            result.Should()
                .BeTrue("objects are the same type");
        }

        [Fact]
        public void IsObjectSameType_DifferentObjectTypes_False()
        {
            object obj = new object();
            string str = string.Empty;

            var result = obj.IsObjectSameType(str);

            result.Should()
                .BeFalse("one object is an object and the other is a string");
        }

        [Fact]
        public void IsObjectSameType_FirstObjectNull_NullReferenceException()
        {
            object o1 = null;
            object o2 = new object();

            Action action = () => o1.IsObjectSameType(o2);

            action
                .ShouldThrow<NullReferenceException>("first object is null");
        }

        [Fact]
        public void IsObjectSameType_SecondObjectNull_False()
        {
            object o1 = new object();
            object o2 = null;

            var result = o1.IsObjectSameType(o2);

            result.Should()
                .BeFalse("Second object is null");
        }
    }
}
