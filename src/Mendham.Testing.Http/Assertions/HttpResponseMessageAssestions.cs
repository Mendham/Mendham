using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Diagnostics;
using System.Net;
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

        /// <summary>
        /// Asserts that the <see cref="HttpResponseMessage"/> has a successful response 
        /// </summary>
        /// <param name="because"> A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see paramref="because" />.</param>
        public AndConstraint<HttpResponseMessageAssestions> HaveSuccessStatusCode(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject != null)
                .BecauseOf(because, becauseArgs)
                .FailWith($"Expected a success status code{{reason}}, but the {nameof(HttpResponseMessage)} was null.");

            Execute.Assertion
                .ForCondition(Subject.IsSuccessStatusCode)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected a success status code{reason}, but found {0}.", Subject.StatusCode.FormattedStatusCode());

            return new AndConstraint<HttpResponseMessageAssestions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="HttpResponseMessage"/> has a status code that matches <paramref name="expected"/>
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because"> A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see paramref="because" />.</param>
        public AndConstraint<HttpResponseMessageAssestions> HaveStatusCode(HttpStatusCode expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject != null)
                .BecauseOf(because, becauseArgs)
                .FailWith($"Expected status code {{0}}{{reason}}, but the {nameof(HttpResponseMessage)} was null.", expected.FormattedStatusCode());

            Execute.Assertion
                .ForCondition(Subject.StatusCode.Equals(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected status code {0}{reason}, but found {1}.", expected.FormattedStatusCode(), Subject.StatusCode.FormattedStatusCode());

            return new AndConstraint<HttpResponseMessageAssestions>(this);
        }

        public HttpContentAssertions Content
        {
            get { return new HttpContentAssertions(Subject.Content); }
        }
    }
}
