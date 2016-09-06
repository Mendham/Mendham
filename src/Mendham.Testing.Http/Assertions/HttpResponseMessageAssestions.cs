using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Diagnostics;
using System.Net.Http;

namespace Mendham.Testing.Http.Assertions
{
    [DebuggerNonUserCode]
    public class HttpResponseMessageAssestions : ReferenceTypeAssertions<HttpResponseMessage, HttpResponseMessageAssestions>
    {
        public HttpResponseMessageAssestions(HttpResponseMessage value)
        {
            Subject = value;
        }

        protected override string Context
        {
            get { return nameof(HttpResponseMessage); }
        }

        public AndConstraint<HttpResponseMessageAssestions> HaveSuccessStatusCode(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected a success status code{reason}, but the {0} was null.", nameof(HttpResponseMessage));

            Execute.Assertion
                .ForCondition(Subject.IsSuccessStatusCode)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected a success status code{reason}, but found {0}.", Subject.StatusCode.ToString());

            return new AndConstraint<HttpResponseMessageAssestions>(this);
        }

        public HttpContentAssertions Content
        {
            get { return new HttpContentAssertions(Subject.Content); }
        }
    }
}
