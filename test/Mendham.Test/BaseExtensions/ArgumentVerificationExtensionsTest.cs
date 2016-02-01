using FluentAssertions;
using Mendham.Test.TestObjects;
using Mendham.Testing;
using System;
using System.Collections.Generic;
using Xunit;

namespace Mendham.Test.BaseExtensions
{
    public class ArgumentVerificationExtensionsTest
    {
        [Theory]
        [InlineMendhamData(null)]
        public void VerifyArgumentNotNull_NullString_ThrowsArgumentNullException(string nullStr)
        {
            Action act = () => nullStr.VerifyArgumentNotNull(nameof(nullStr));

            act.ShouldThrow<ArgumentNullException>()
                .And.ParamName.ShouldBeEquivalentTo("nullStr");
        }

        [Theory]
        [InlineMendhamData(null)]
        public void VerifyArgumentNotNull_NullStringWithMessage_ThrowsArgumentNullException(string nullStr, string msg)
        {
            Action act = () => nullStr.VerifyArgumentNotNull(nameof(nullStr), msg);

            act.ShouldThrow<ArgumentNullException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(nullStr));
        }

        [Theory]
        [InlineMendhamData(null)]
        public void VerifyArgumentNotNull_NullStringWithCustomException_ThrowsCustomException(string nullStr, string msg)
        {
            Action act = () => nullStr.VerifyArgumentNotNull(a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, nullStr));

        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNull_Int_Passes(int? obj)
        {
            var result = obj.VerifyArgumentNotNull(nameof(obj));

            result.Should().Be(obj);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNull_IntWithMessage_Passes(int? obj, string msg)
        {
            var result = obj.VerifyArgumentNotNull(nameof(obj), msg);

            result.Should().Be(obj);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNull_IntWithCustomException_Passes(int? obj, string msg)
        {
            var result = obj.VerifyArgumentNotNull(a => new CustomException(msg, a));

            result.Should().Be(obj);
        }

        [Theory]
        [InlineMendhamData(null)]
        public void VerifyArgumentNotDefault_NullInt_ThrowsArgumentNullException(int? nullVal)
        {
            Action act = () => nullVal.VerifyArgumentNotDefaultValue(nameof(nullVal));

            act.ShouldThrow<ArgumentNullException>()
                .And.ParamName.ShouldBeEquivalentTo("nullVal");
        }

        [Theory]
        [InlineMendhamData(null)]
        public void VerifyArgumentNotDefault_NullIntWithMessage_ThrowsArgumentNullException(int? nullVal, string msg)
        {
            Action act = () => nullVal.VerifyArgumentNotDefaultValue(nameof(nullVal), msg);

            act.ShouldThrow<ArgumentNullException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(nullVal));
        }

        [Theory]
        [InlineMendhamData(null)]
        public void VerifyArgumentNotDefault_NullIntWithCustomException_ThrowsCustomException(int? nullVal, string msg)
        {
            Action act = () => nullVal.VerifyArgumentNotDefaultValue(a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, nullVal));
        }

        [Theory]
        [InlineMendhamData(default(int))]
        public void VerifyArgumentNotDefault_DefaultInt_ThrowsArgumentException(int defaultInt)
        {
            Action act = () => defaultInt.VerifyArgumentNotDefaultValue(nameof(defaultInt));

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.ShouldBeEquivalentTo(nameof(defaultInt));
        }

        [Theory]
        [InlineMendhamData(default(int))]
        public void VerifyArgumentNotDefault_DefaultIntWithMessage_ThrowsArgumentException(int defaultInt, string msg)
        {
            Action act = () => defaultInt.VerifyArgumentNotDefaultValue(nameof(defaultInt), msg);

            act.ShouldThrow<ArgumentException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(defaultInt));
        }

        [Theory]
        [InlineMendhamData(default(int))]
        public void VerifyArgumentNotDefault_DefaultIntWithCustomException_ThrowsCustomException(int defaultInt, string msg)
        {
            Action act = () => defaultInt.VerifyArgumentNotDefaultValue(a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, defaultInt));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotDefault_Int_Passes(int val)
        {
            var result = val.VerifyArgumentNotDefaultValue(nameof(val));

            result.Should().Be(val);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotDefault_IntWithCustomException_Passes(int val, string msg)
        {
            var result = val.VerifyArgumentNotDefaultValue(a => new CustomException(msg, a));

            result.Should().Be(val);
        }

        [Theory]
        [InlineMendhamData(default(string))]
        public void VerifyArgumentNotDefault_DefaultString_ThrowsArgumentNullException(string defaultStr)
        {
            Action act = () => defaultStr.VerifyArgumentNotDefaultValue(nameof(defaultStr));

            act.ShouldThrow<ArgumentNullException>()
                .And.ParamName.ShouldBeEquivalentTo(nameof(defaultStr));
        }

        [Theory]
        [InlineMendhamData(default(string))]
        public void VerifyArgumentNotDefault_DefaultStringWithMessage_ThrowsArgumentNullException(string defaultStr, string msg)
        {
            Action act = () => defaultStr.VerifyArgumentNotDefaultValue(nameof(defaultStr), msg);

            act.ShouldThrow<ArgumentNullException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(defaultStr));
        }

        [Theory]
        [InlineMendhamData(default(string))]
        public void VerifyArgumentNotDefault_DefaultStringWithCustomException_ThrowsCustomException(string defaultStr, string msg)
        {
            Action act = () => defaultStr.VerifyArgumentNotDefaultValue(a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, defaultStr));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotDefault_String_Passes(string val)
        {
            var result = val.VerifyArgumentNotDefaultValue(nameof(val));

            result.Should().Be(val);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotDefault_StringWithMessage_Passes(string val, string msg)
        {
            var result = val.VerifyArgumentNotDefaultValue(nameof(val), msg);

            result.Should().Be(val);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotDefault_StringWithCustomException_Passes(string val, string msg)
        {
            var result = val.VerifyArgumentNotDefaultValue(a => new CustomException(msg, a));

            result.Should().Be(val);
        }

        [Fact]
        public void VerifyArgumentNotNullOrEmpty_NullCollection_ThrowsArgumentNullException()
        {
            List<int> vals = null;

            Action act = () => vals.VerifyArgumentNotNullOrEmpty(nameof(vals));

            act.ShouldThrow<ArgumentNullException>()
                .And.ParamName.ShouldBeEquivalentTo(nameof(vals));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_NullCollectionWithMessage_ThrowsArgumentNullException(string msg)
        {
            List<int> vals = null;

            Action act = () => vals.VerifyArgumentNotNullOrEmpty(nameof(vals), msg);

            act.ShouldThrow<ArgumentNullException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(vals));
        }

        [Theory, MendhamData]
        public void VerifyArgumentNotNullOrEmpty_NullCollectionCustomException_ThrowsCustomException(string msg)
        {
            List<int> vals = null;

            Action act = () => vals.VerifyArgumentNotNullOrEmpty(a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, vals));
        }

        [Fact]
        public void VerifyArgumentNotNullOrEmpty_EmptyCollection_ThrowsArgumentException()
        {
            List<int> vals = new List<int>();

            Action act = () => vals.VerifyArgumentNotNullOrEmpty(nameof(vals));

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.ShouldBeEquivalentTo(nameof(vals));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_EmptyCollectionWithMessage_ThrowsArgumentException(string msg)
        {
            List<int> vals = new List<int>();

            Action act = () => vals.VerifyArgumentNotNullOrEmpty(nameof(vals), msg);

            act.ShouldThrow<ArgumentException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(vals));
        }

        [Theory, MendhamData]
        public void VerifyArgumentNotNullOrEmpty_EmptyCollectionCustomException_ThrowsCustomException(string msg)
        {
            List<int> vals = new List<int>();

            Action act = () => vals.VerifyArgumentNotNullOrEmpty(a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, vals));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_NonEmptyCollection_Passes(List<int> vals)
        {
            var result = vals.VerifyArgumentNotNullOrEmpty(nameof(vals));

            result.Should().ContainInOrder(vals);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_NonEmptyCollectionWithMessage_Passes(List<int> vals, string msg)
        {
            var result = vals.VerifyArgumentNotNullOrEmpty(nameof(vals), msg);

            result.Should().ContainInOrder(vals);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_NonEmptyCollectionWithCustomException_Passes(List<int> vals, string msg)
        {
            var result = vals.VerifyArgumentNotNullOrEmpty(a => new CustomException(msg, a));

            result.Should().ContainInOrder(vals);
        }

        [Fact]
        public void VerifyArgumentNotNullOrEmpty_NullString_ThrowsArgumentNullException()
        {
            string val = null;

            Action act = () => val.VerifyArgumentNotNullOrEmpty(nameof(val));

            act.ShouldThrow<ArgumentNullException>()
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_NullStringWithMessage_ThrowsArgumentNullException(string msg)
        {
            string val = null;

            Action act = () => val.VerifyArgumentNotNullOrEmpty(nameof(val), msg);

            act.ShouldThrow<ArgumentNullException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_NullStringWithCustomException_ThrowsCustomException(string msg)
        {
            string val = null;

            Action act = () => val.VerifyArgumentNotNullOrEmpty(a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, val));
        }

        [Fact]
        public void VerifyArgumentNotNullOrEmpty_EmptyStringWith_ThrowsArgumentException()
        {
            string val = string.Empty;

            Action act = () => val.VerifyArgumentNotNullOrEmpty(nameof(val));

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_EmptyStringWithMessage_ThrowsArgumentException(string msg)
        {
            string val = string.Empty;

            Action act = () => val.VerifyArgumentNotNullOrEmpty(nameof(val), msg);

            act.ShouldThrow<ArgumentException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_EmptyStringWithCustomException_ThrowsCustomException(string msg)
        {
            string val = string.Empty;

            Action act = () => val.VerifyArgumentNotNullOrEmpty(a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_NonEmptyString_Passes(string val)
        {
            var result = val.VerifyArgumentNotNullOrEmpty(nameof(val));

            result.Should().Be(val);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_NonEmptyStringWithMessage_Passes(string val, string msg)
        {
            var result = val.VerifyArgumentNotNullOrEmpty(nameof(val), msg);

            result.Should().Be(val);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_NonEmptyStringWithCustomException_Passes(string val, string msg)
        {
            var result = val.VerifyArgumentNotNullOrEmpty(a => new CustomException(msg, a));

            result.Should().Be(val);
        }

        [Fact]
        public void VerifyArgumentNotNullOrWhiteSpace_NullString_ThrowsArgumentNullException()
        {
            string val = null;

            Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(nameof(val));

            act.ShouldThrow<ArgumentNullException>()
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrWhiteSpace_NullStringWithMessage_ThrowsArgumentNullException(string msg)
        {
            string val = null;

            Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(nameof(val), msg);

            act.ShouldThrow<ArgumentNullException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrWhiteSpace_NullStringWithCustomException_ThrowsCustomException(string msg)
        {
            string val = null;

            Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, val));
        }

        [Fact]
        public void VerifyArgumentNotNullOrWhiteSpace_EmptyString_ThrowsArgumentException()
        {
            string val = string.Empty;

            Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(nameof(val));

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrWhiteSpace_EmptyStringWithMessage_ThrowsArgumentException(string msg)
        {
            string val = string.Empty;

            Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(nameof(val), msg);

            act.ShouldThrow<ArgumentException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrWhiteSpace_EmptyStringWithCustomException_ThrowsCustomException(string msg)
        {
            string val = string.Empty;

            Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, val));
        }

        [Fact]
        public void VerifyArgumentNotNullOrWhiteSpace_WhiteSpaceString_ThrowsArgumentException()
        {
            string val = "   ";

            Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(nameof(val));

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrWhiteSpace_WhiteSpaceStringWithMessage_ThrowsArgumentException(string msg)
        {
            string val = "   ";

            Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(nameof(val), msg);

            act.ShouldThrow<ArgumentException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrWhiteSpace_WhiteSpaceStringWithCustomException_ThrowsCustomException(string msg)
        {
            string val = "   ";

            Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, val));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrWhiteSpace_ValidString_Passes(string val)
        {
            var result = val.VerifyArgumentNotNullOrWhiteSpace(nameof(val));

            result.Should().Be(val);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrWhiteSpace_ValidStringWithMessage_Passes(string val, string msg)
        {
            var result = val.VerifyArgumentNotNullOrWhiteSpace(nameof(val), msg);

            result.Should().Be(val);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrWhiteSpace_ValidStringWithCustomException_Passes(string val, string msg)
        {
            var result = val.VerifyArgumentNotNullOrWhiteSpace(a => new CustomException(msg, a));

            result.Should().Be(val);
        }

        [Theory]
        [InlineMendhamData("test", 1, 5, false)]
        [InlineMendhamData("test", null, 5, false)]
        [InlineMendhamData("test", 1, null, false)]
        [InlineMendhamData(" test  ", 1, 5, true)]
        public void VerifyArgumentLength_ValidCase_Passes(string val, int? minimum, int? maximum, bool trimFirst)
        {
            var result = val.VerifyArgumentLength(minimum, maximum, trimFirst, nameof(val));

            result.Should().Be(val);
        }

        [Theory]
        [InlineMendhamData("test", 1, 5, false)]
        [InlineMendhamData("test", null, 5, false)]
        [InlineMendhamData("test", 1, null, false)]
        [InlineMendhamData(" test  ", 1, 5, true)]
        public void VerifyArgumentLength_ValidCaseWithMessage_Passes(string val, int? minimum, int? maximum, bool trimFirst, string msg)
        {
            var result = val.VerifyArgumentLength(minimum, maximum, trimFirst, nameof(val));

            result.Should().Be(val);
        }

        [Theory]
        [InlineMendhamData(5, 10, false)]
        [InlineMendhamData(null, 3, false)]
        [InlineMendhamData(5, null, false)]
        [InlineMendhamData(5, 10, true)]
        public void VerifyArgumentLength_NullInvalidCase_ThrowsArgumentNullException(int? minimum, int? maximum, bool trimFirst)
        {
            string val = null;

            Action act = () => val.VerifyArgumentLength(minimum, maximum, trimFirst, nameof(val));

            act.ShouldThrow<ArgumentNullException>()
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [InlineMendhamData(5, 10, false)]
        [InlineMendhamData(null, 3, false)]
        [InlineMendhamData(5, null, false)]
        [InlineMendhamData(5, 10, true)]
        public void VerifyArgumentLength_NullInvalidCaseWithMessage_ThrowsArgumentNullException(int? minimum, int? maximum, bool trimFirst, string msg)
        {
            string val = null;

            Action act = () => val.VerifyArgumentLength(minimum, maximum, trimFirst, nameof(val), msg);

            act.ShouldThrow<ArgumentNullException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [InlineMendhamData(5, 10, false)]
        [InlineMendhamData(null, 3, false)]
        [InlineMendhamData(5, null, false)]
        [InlineMendhamData(5, 10, true)]
        public void VerifyArgumentLength_NullInvalidCaseWithCustomException_ThrowsCustomException(int? minimum, int? maximum, bool trimFirst, string msg)
        {
            string val = null;

            Action act = () => val.VerifyArgumentLength(minimum, maximum, trimFirst, a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, val));
        }

        [Theory]
        [InlineMendhamData("test", 5, 10, false)]
        [InlineMendhamData("test", null, 3, false)]
        [InlineMendhamData("test", 5, null, false)]
        [InlineMendhamData(" test  ", 5, 10, true)]
        public void VerifyArgumentLength_InvalidCase_ThrowsArgumentException(string val, int? minimum, int? maximum, bool trimFirst)
        {
            Action act = () => val.VerifyArgumentLength(minimum, maximum, trimFirst, nameof(val));

            act.ShouldThrow<ArgumentException>()
                .Where(a => a.Message.Contains(val))
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [InlineMendhamData("test", 5, 10, false)]
        [InlineMendhamData("test", null, 3, false)]
        [InlineMendhamData("test", 5, null, false)]
        [InlineMendhamData(" test  ", 5, 10, true)]
        public void VerifyArgumentLength_InvalidCaseWithMessage_ThrowsArgumentException(string val, int? minimum, int? maximum, bool trimFirst, string msg)
        {
            Action act = () => val.VerifyArgumentLength(minimum, maximum, trimFirst, nameof(val), msg);

            act.ShouldThrow<ArgumentException>()
                .Where(a => a.Message.Contains(msg))
                .Where(a => a.Message.Contains(val))
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [InlineMendhamData("test", 5, 10, false)]
        [InlineMendhamData("test", null, 3, false)]
        [InlineMendhamData("test", 5, null, false)]
        [InlineMendhamData(" test  ", 5, 10, true)]
        public void VerifyArgumentLength_InvalidCaseWithCustomException_ThrowsCustomException(string val, int? minimum, int? maximum, bool trimFirst, string msg)
        {
            Action act = () => val.VerifyArgumentLength(minimum, maximum, trimFirst, a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, val));
        }

        [Theory]
        [InlineMendhamData(4, 1, 5)]
        [InlineMendhamData(1, 1, 5)]
        [InlineMendhamData(5, 1, 5)]
        [InlineMendhamData(-1, -2, 5)]
        [InlineMendhamData(-1, null, 5)]
        [InlineMendhamData(5, null, 5)]
        [InlineMendhamData(4, 1, null)]
        [InlineMendhamData(1, 1, null)]
        public void VerifyArgumentRange_ValidCase_Passes(int val, int? minimum, int? maximum)
        {
            var result = val.VerifyArgumentRange(minimum, maximum, nameof(val));

            result.Should().Be(val);
        }

        [Theory]
        [InlineMendhamData(4, 1, 5)]
        [InlineMendhamData(1, 1, 5)]
        [InlineMendhamData(5, 1, 5)]
        [InlineMendhamData(-1, -2, 5)]
        [InlineMendhamData(-1, null, 5)]
        [InlineMendhamData(5, null, 5)]
        [InlineMendhamData(4, 1, null)]
        [InlineMendhamData(1, 1, null)]
        public void VerifyArgumentRange_ValidCaseWithMessage_Passes(int val, int? minimum, int? maximum, string msg)
        {
            var result = val.VerifyArgumentRange(minimum, maximum, nameof(val), msg);

            result.Should().Be(val);
        }

        [Theory]
        [InlineMendhamData(4, 1, 5)]
        [InlineMendhamData(1, 1, 5)]
        [InlineMendhamData(5, 1, 5)]
        [InlineMendhamData(-1, -2, 5)]
        [InlineMendhamData(-1, null, 5)]
        [InlineMendhamData(5, null, 5)]
        [InlineMendhamData(4, 1, null)]
        [InlineMendhamData(1, 1, null)]
        public void VerifyArgumentRange_ValidCaseWithCustomException_Passes(int val, int? minimum, int? maximum, string msg)
        {
            var result = val.VerifyArgumentRange(minimum, maximum, a => new CustomException(msg, a));

            result.Should().Be(val);
        }

        [Theory]
        [InlineMendhamData(0, 1, 5)]
        [InlineMendhamData(6, 1, 5)]
        [InlineMendhamData(6, null, 5)]
        [InlineMendhamData(10, null, 5)]
        [InlineMendhamData(0, 1, null)]
        [InlineMendhamData(-1, 1, null)]
        public void VerifyArgumentRange_InvalidCase_ThrowsArgumentOutOfRangeException(int val, int? minimum, int? maximum)
        {
            Action act = () => val.VerifyArgumentRange(minimum, maximum, nameof(val));

            act.ShouldThrow<ArgumentOutOfRangeException>()
                .Where(a => Equals(a.ActualValue, val))
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [InlineMendhamData(0, 1, 5)]
        [InlineMendhamData(6, 1, 5)]
        [InlineMendhamData(6, null, 5)]
        [InlineMendhamData(10, null, 5)]
        [InlineMendhamData(0, 1, null)]
        [InlineMendhamData(-1, 1, null)]
        public void VerifyArgumentRange_InvalidCaseWithMessage_ThrowsArgumentOutOfRangeException(int val, int? minimum, int? maximum, string msg)
        {
            Action act = () => val.VerifyArgumentRange(minimum, maximum, nameof(val), msg);

            act.ShouldThrow<ArgumentOutOfRangeException>()
                .Where(a => Equals(a.ActualValue, val))
                .Where(a => a.Message.Contains(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [InlineMendhamData(0, 1, 5)]
        [InlineMendhamData(6, 1, 5)]
        [InlineMendhamData(6, null, 5)]
        [InlineMendhamData(10, null, 5)]
        [InlineMendhamData(0, 1, null)]
        [InlineMendhamData(-1, 1, null)]
        public void VerifyArgumentRange_InvalidCaseWithCustomException_ThrowsCustomException(int val, int? minimum, int? maximum, string msg)
        {
            Action act = () => val.VerifyArgumentRange(minimum, maximum, a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, val));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentMeetsCriteria_ValidCriteria_Passes(int val, string msg)
        {
            var result = val.VerifyArgumentMeetsCriteria(a => true, nameof(val), msg);

            result.Should().Be(val);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentMeetsCriteria_InvalidCriteria_ThrowsArgumentException(string val, string msg)
        {
            Action act = () => val.VerifyArgumentMeetsCriteria(a => false, nameof(val), msg);

            act.ShouldThrow<ArgumentException>()
                .Where(a => a.Message.Contains(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(val));
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentMeetsCriteria_InvalidCriteriaWithCustomException_ThrowsCustomException(string val, string msg)
        {
            Action act = () => val.VerifyArgumentMeetsCriteria(a => false, a => new CustomException(msg, a));

            act.ShouldThrow<CustomException>()
                .WithMessage(msg)
                .Where(a => Equals(a.Value, val));
        }
    }
}
