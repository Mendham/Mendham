using System;

namespace Mendham.Infrastructure.Http.Test.TestObjects
{
    public class ObjectWithReadOnlyProperties
    {
        public ObjectWithReadOnlyProperties(string value1, int value2)
        {
            Value1 = value1;
            Value2 = value2;
            ValueNotToSerialize = Guid.NewGuid();
        }

        public string Value1 { get; }
        public int Value2 { get; }

        public Guid ValueNotToSerialize { get; set; }

        public const string FormatString = "{{\"value1\":\"{0}\",\"value2\":{1}}}";
        public const string FormatStringWithAdditionalContent = "{{\"value1\":\"{0}\",\"value2\":{1},\"valueToNotSerialize\":\"{2}\"}}";
    }
}
