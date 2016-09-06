using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Json;
using FluentAssertions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mendham.Testing.Http.Assertions
{
    [DebuggerNonUserCode]
    public class HttpContentAssertions : ReferenceTypeAssertions<HttpContent, HttpContentAssertions>
    {
        public HttpContentAssertions(HttpContent value)
        {
            Subject = value;
        }

        private static readonly TaskFactory _taskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);
        private static readonly JTokenFormatter _jTokenFormatter = new JTokenFormatter();

        public AndConstraint<HttpContentAssertions> HaveMediaType(string mediaType, string because = "", params object[] becauseArgs)
        {
            mediaType.VerifyArgumentNotNullOrEmpty(nameof(mediaType), "Media type is required");

            ValidateHttpContentNotNull(because, becauseArgs, "Expected media type {0}{reason}", mediaType);

            Execute.Assertion
                .ForCondition(string.Equals(Subject.Headers.ContentType?.MediaType, mediaType, StringComparison.OrdinalIgnoreCase))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected media type {0}{reason}, but found {1}.", mediaType, Subject.Headers.ContentType?.MediaType);

            return new AndConstraint<HttpContentAssertions>(this);
        }

        public AndConstraint<HttpContentAssertions> HaveJsonMediaType(string because = "", params object[] becauseArgs)
        {
            return HaveMediaType("application/json", because, becauseArgs);
        }

        public AndConstraint<HttpContentAssertions> BeString(string expected, string because = "", params object[] becauseArgs)
        {
            string expectedMessage = "Expected string {0}{reason}";

            ValidateHttpContentNotNull(because, becauseArgs, expectedMessage, expected);

            var content = GetStringContent();

            Execute.Assertion
                .ForCondition(string.Equals(content, expected, StringComparison.Ordinal))
                .BecauseOf(because, becauseArgs)
                .FailWith(expectedMessage +  ", but found {1}.", expected, content);

            return new AndConstraint<HttpContentAssertions>(this);
        }

        private const string ExpectedJsonBe = "Expected json content to be {0}{{reason}}";

        public AndConstraint<HttpContentAssertions> HaveJsonBe(JToken expected, string because = "", params object[] becauseArgs)
        {
            JToken jToken = GetJTokenContent(because, becauseArgs, ExpectedJsonBe, expected);

            var formattedError = string.Format(ExpectedJsonBe + ", but found {1}.",
                _jTokenFormatter.ToString(expected), _jTokenFormatter.ToString(jToken));

            Execute.Assertion
                .ForCondition(JToken.DeepEquals(jToken, expected))
                .BecauseOf(because, becauseArgs)
                .FailWith(formattedError);

            return new AndConstraint<HttpContentAssertions>(this);
        }

        private const string ExpectedJsonEquivalentTo = "Expected json content to be equivalent to {0}{reason}";

        public AndConstraint<HttpContentAssertions> HaveJsonEquivalentTo<T>(T expected, string because = "", params object[] becauseArgs)
        {
            return HaveJsonEquivalentTo<T>(expected, EqualityComparer<T>.Default, because, becauseArgs);
        }

        public AndConstraint<HttpContentAssertions> HaveJsonEquivalentTo<T>(T expected, IEqualityComparer<T> comparer, string because = "", params object[] becauseArgs)
        {
            comparer.VerifyArgumentNotNull(nameof(comparer));

            T obj = GetJsonContent<T>(because, becauseArgs, ExpectedJsonBe, expected);

            Execute.Assertion
                .ForCondition(comparer.Equals(obj, expected))
                .BecauseOf(because, becauseArgs)
                .FailWith(ExpectedJsonEquivalentTo + ", but found {1}.", expected, obj);

            return new AndConstraint<HttpContentAssertions>(this);
        }

        private const string ExpectedContentMatch = "Expected content to match {0}{reason}";

        public AndConstraint<HttpContentAssertions> HaveJsonMatch(Expression<Func<JToken, bool>> predicate, string because = "", params object[] becauseArgs)
        {
            var jToken = GetJTokenContent(because, becauseArgs, ExpectedContentMatch, predicate.Body);

            Execute.Assertion
                .ForCondition(predicate.Compile()(jToken))
                .BecauseOf(because, becauseArgs)
                .FailWith(ExpectedContentMatch, predicate.Body);

            return new AndConstraint<HttpContentAssertions>(this);
        }

        public AndConstraint<HttpContentAssertions> HaveJsonMatch<T>(Expression<Func<T, bool>> predicate, string because = "", params object[] becauseArgs)
        {
            var obj = GetJsonContent<T>(because, becauseArgs, ExpectedContentMatch, predicate.Body);

            Execute.Assertion
                .ForCondition(predicate.Compile()(obj))
                .BecauseOf(because, becauseArgs)
                .FailWith(ExpectedContentMatch, predicate.Body);

            return new AndConstraint<HttpContentAssertions>(this);
        }
       
        protected override string Context
        {
            get { return nameof(HttpContent); }
        }

        private string GetStringContent()
        {
            Func<Task<string>> getContentString = () => Subject.ReadAsStringAsync();
            return _taskFactory.StartNew(getContentString).Unwrap().GetAwaiter().GetResult();
        }


        private T GetJsonContent<T>(string because, object[] becauseArgs, string expected, params object[] expectedArgs)
        {
            ValidateJsonMediaType(because, becauseArgs, expected, expectedArgs);

            string contentString = GetStringContent();

            return JsonConvert.DeserializeObject<T>(contentString);
        }

        private JToken GetJTokenContent(string because, object[] becauseArgs, string expected, params object[] expectedArgs)
        {
            ValidateJsonMediaType(because, becauseArgs, expected, expectedArgs);

            string contentString = GetStringContent();

            return JToken.Parse(contentString);
        }

        private void ValidateHttpContentNotNull(string because, object[] becauseArgs, string expectedMessage, params object[] expectedArgs)
        {
            if (Subject == null)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(expectedMessage + ", but HttpContent was null.", expectedArgs);
            }
        }

        private void ValidateJsonMediaType(string because, object[] becauseArgs, string expectedMessage, params object[] expectedArgs)
        {
            ValidateHttpContentNotNull(because, becauseArgs, expectedMessage, expectedArgs);

            var contentMediaType = Subject.Headers.ContentType?.MediaType;

            var failString = $"{expectedMessage}, but media type {{{expectedArgs.Count()}}} is invalid for json content.";

            var failArgs = expectedArgs.ToList();
            failArgs.Add(contentMediaType);
                

            Execute.Assertion
                .ForCondition(string.Equals("application/json", contentMediaType, StringComparison.Ordinal))
                .BecauseOf(because, becauseArgs)
                .FailWith(failString, failArgs.ToArray());
        }
    }
}
