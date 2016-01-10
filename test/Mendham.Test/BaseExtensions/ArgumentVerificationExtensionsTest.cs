using FluentAssertions;
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
        [InlineMendhamData(null)]
        public void VerifyArguementNotDefault_NullInt_ThrowsArgumentNullException(int? nullVal)
        {
            Action act = () => nullVal.VerifyArgumentNotDefaultValue(nameof(nullVal));

            act.ShouldThrow<ArgumentNullException>()
                .And.ParamName.ShouldBeEquivalentTo("nullVal");
        }

        [Theory]
        [InlineMendhamData(null)]
        public void VerifyArguementNotDefault_NullIntWithMessage_ThrowsArgumentNullException(int? nullVal, string msg)
        {
            Action act = () => nullVal.VerifyArgumentNotDefaultValue(nameof(nullVal), msg);

            act.ShouldThrow<ArgumentNullException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(nullVal));
        }

        [Theory]
        [InlineMendhamData(default(int))]
        public void VerifyArguementNotDefault_DefaultInt_ThrowsArgumentException(int defaultInt)
        {
            Action act = () => defaultInt.VerifyArgumentNotDefaultValue(nameof(defaultInt));

            act.ShouldThrow<ArgumentException>()
                .And.ParamName.ShouldBeEquivalentTo(nameof(defaultInt));
        }

        [Theory]
        [InlineMendhamData(default(int))]
        public void VerifyArguementNotDefault_DefaultIntWithMessage_ThrowsArgumentException(int defaultInt, string msg)
        {
            Action act = () => defaultInt.VerifyArgumentNotDefaultValue(nameof(defaultInt), msg);

            act.ShouldThrow<ArgumentException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(defaultInt));
        }

        [Theory]
        [InlineMendhamData(default(string))]
        public void VerifyArguementNotDefault_DefaultString_ThrowsArgumentNullException(string defaultStr)
        {
            Action act = () => defaultStr.VerifyArgumentNotDefaultValue(nameof(defaultStr));

            act.ShouldThrow<ArgumentNullException>()
                .And.ParamName.ShouldBeEquivalentTo(nameof(defaultStr));
        }

        [Theory]
        [InlineMendhamData(default(string))]
        public void VerifyArguementNotDefault_DefaultStringWithMessage_ThrowsArgumentNullException(string defaultStr, string msg)
        {
            Action act = () => defaultStr.VerifyArgumentNotDefaultValue(nameof(defaultStr), msg);

            act.ShouldThrow<ArgumentNullException>()
                .Where(a => a.Message.StartsWith(msg))
                .And.ParamName.ShouldBeEquivalentTo(nameof(defaultStr));
        }

        [Theory]
        [MendhamData]
        public void VerifyArguementNotDefault_String_Passes(string val)
        {
            var result = val.VerifyArgumentNotDefaultValue(nameof(val));

            result.Should().Be(val);
        }

        [Theory]
        [MendhamData]
        public void VerifyArguementNotDefault_StringWithMessage_Passes(string val, string msg)
        {
            var result = val.VerifyArgumentNotDefaultValue(nameof(val), msg);

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
        public void VerifyArgumentNotNullOrEmpty_NullString_Throws(string msg)
        {
            string val = null;

            Action act = () => val.VerifyArgumentNotNullOrEmpty(msg);

            act.ShouldThrow<ArgumentException>()
                .WithMessage(msg);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_EmptyString_Throws(string msg)
        {
            string val = string.Empty;

            Action act = () => val.VerifyArgumentNotNullOrEmpty(msg);

            act.ShouldThrow<ArgumentException>()
                .WithMessage(msg);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrEmpty_NonEmptyString_Passes(string val, string msg)
        {
            var result = val.VerifyArgumentNotNullOrEmpty(msg);

            result.Should().Be(val);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrWhiteSpace_NullString_Throws(string msg)
        {
            string val = null;

            Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(msg);

            act.ShouldThrow<ArgumentException>()
                .WithMessage(msg);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrWhiteSpac_EmptyString_Throws(string msg)
        {
            string val = string.Empty;

            Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(msg);

            act.ShouldThrow<ArgumentException>()
                .WithMessage(msg);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrWhiteSpace_WhiteSpaceString_Throws(string msg)
        {
            string val = "   ";

            Action act = () => val.VerifyArgumentNotNullOrWhiteSpace(msg);

            act.ShouldThrow<ArgumentException>()
                .WithMessage(msg);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentNotNullOrWhiteSpace_ValidString_Passes(string val, string msg)
        {
            var result = val.VerifyArgumentNotNullOrWhiteSpace(msg);

            result.Should().Be(val);
        }

        [Theory]
        [InlineMendhamData("test", 1, 5, false)]
        [InlineMendhamData("test", null, 5, false)]
        [InlineMendhamData("test", 1, null, false)]
        [InlineMendhamData(" test  ", 1, 5, true)]
        public void VerifyArgumentLength_ValidCase_Passes(string val, int? minimum, int? maximum, bool trimFirst, string msg)
        {
            var result = val.VerifyArgumentLength(minimum, maximum, trimFirst, msg);

            result.Should().Be(val);
        }

        [Theory]
        [InlineMendhamData("test", 5, 10, false)]
        [InlineMendhamData("test", null, 3, false)]
        [InlineMendhamData("test", 5, null, false)]
        [InlineMendhamData(" test  ", 5, 10, true)]
        public void VerifyArgumentLength_InvalidCase_Throws(string val, int? minimum, int? maximum, bool trimFirst, string msg)
        {
            Action act = () => val.VerifyArgumentLength(minimum, maximum, trimFirst, msg);

            act.ShouldThrow<ArgumentException>()
                .WithMessage(msg);
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
        public void VerifyArgumentRange_ValidCase_Passes(int val, int? minimum, int? maximum, string msg)
        {
            var result = val.VerifyArgumentRange(minimum, maximum, msg);

            result.Should().Be(val);
        }

        [Theory]
        [InlineMendhamData(0, 1, 5)]
        [InlineMendhamData(6, 1, 5)]
        [InlineMendhamData(6, null, 5)]
        [InlineMendhamData(10, null, 5)]
        [InlineMendhamData(0, 1, null)]
        [InlineMendhamData(-1, 1, null)]
        public void VerifyArgumentRange_InvalidCase_Throws(int val, int? minimum, int? maximum, string msg)
        {
            Action act = () => val.VerifyArgumentRange(minimum, maximum, msg);

            act.ShouldThrow<ArgumentException>()
                .WithMessage(msg);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentMeetsCriteria_ValidCriteria_Passes(int val, string msg)
        {
            var result = val.VerifyArgumentMeetsCriteria(a => true, msg);

            result.Should().Be(val);
        }

        [Theory]
        [MendhamData]
        public void VerifyArgumentMeetsCriteria_InvalidCriteria_Throws(string val, string msg)
        {
            Action act = () => val.VerifyArgumentMeetsCriteria(a => false, msg);

            act.ShouldThrow<ArgumentException>()
                .WithMessage(msg);
        }
    }
}
