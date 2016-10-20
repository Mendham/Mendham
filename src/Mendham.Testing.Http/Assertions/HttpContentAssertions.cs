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
        private const string JsonMediaType = "application/json";

        /// <summary>
        ///   Asserts that the media type found in the header of the <see cref="HttpContent"/> is equivalent to 
        ///   <paramref name="mediaType"/>. 
        /// </summary>
        /// <param name="mediaType">
        ///   The expected media type.
        /// </param>
        /// <param name = "because">
        ///   A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "becauseArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "because" />.
        /// </param>
        public AndConstraint<HttpContentAssertions> HaveMediaType(string mediaType, string because = "", params object[] becauseArgs)
        {
            mediaType.VerifyArgumentNotNullOrEmpty(nameof(mediaType), "Media type is required");

            ValidateHttpContentNotNull(because, becauseArgs, "Expected media type {0}{reason}", mediaType);

            Execute.Assertion
                .ForCondition(IsMediaType(Subject.Headers.ContentType?.MediaType, mediaType))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected media type {0}{reason}, but found {1}.", mediaType, Subject.Headers.ContentType?.MediaType);

            return new AndConstraint<HttpContentAssertions>(this);
        }

        /// <summary>
        ///   Asserts that the media type found in the header of the <see cref="HttpContent"/> is equivalent to "application/json".
        /// </summary>
        /// <param name = "because">
        ///   A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "becauseArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "because" />.
        /// </param>
        public AndConstraint<HttpContentAssertions> HaveJsonMediaType(string because = "", params object[] becauseArgs)
        {
            return HaveMediaType(JsonMediaType, because, becauseArgs);
        }

        /// <summary>
        ///   Asserts that the <see cref="HttpContent"/> contains a string that is exactly the same as <paramref name="expected"/>.
        /// </summary>
        /// <param name="expected">
        ///   The expected string.
        /// </param>
        /// <param name = "because">
        ///   A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "becauseArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "because" />.
        /// </param>
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

        /// <summary>
        ///   Asserts that the <see cref="HttpContent"/> contains a string that matches the condition in <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">
        ///   A predicate to match the string content against.
        /// </param>
        /// <param name = "because">
        ///   A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "becauseArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "because" />.
        /// </param>
        public AndConstraint<HttpContentAssertions> HaveStringMatch(Expression<Func<string, bool>> predicate, string because = "", params object[] becauseArgs)
        {
            string expectedMessage = "Expected string to match {0}{reason}";

            ValidateHttpContentNotNull(because, becauseArgs, expectedMessage, predicate.Body);

            var content = GetStringContent();

            Execute.Assertion
                .ForCondition(predicate.Compile()(content))
                .BecauseOf(because, becauseArgs)
                .FailWith(expectedMessage + $", but \"{content}\" does not.", predicate.Body);

            return new AndConstraint<HttpContentAssertions>(this);
        }

        private const string ExpectedJsonBe = "Expected json content to be {0}{{reason}}";

        /// <summary>
        ///   Asserts that the <see cref="HttpContent"/> contains JSON that equals <paramref name="expected"/>.
        /// </summary>
        /// <param name="expected">
        ///   The expected element.
        /// </param>
        /// <param name = "because">
        ///   A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "becauseArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "because" />.
        /// </param>
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

        /// <summary>
        ///   Asserts that the <see cref="HttpContent"/> contains JSON that when deserialized into <typeparamref name="T"/> is
        ///   equivalent to <paramref name="expected"/>.
        /// </summary>
        /// <typeparam name="T">
        ///   The type to deserialize the JSON into.
        /// </typeparam>
        /// <param name="expected">
        ///   The expected object.
        /// </param>
        /// <param name = "because">
        ///   A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "becauseArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "because" />.
        /// </param>
        public AndConstraint<HttpContentAssertions> HaveJsonEquivalentTo<T>(T expected, string because = "", params object[] becauseArgs)
        {
            return HaveJsonEquivalentTo<T>(expected, EqualityComparer<T>.Default, because, becauseArgs);
        }

        /// <summary>
        ///   Asserts that the <see cref="HttpContent"/> contains JSON that when deserialized into <typeparamref name="T"/> is
        ///   equivalent to <paramref name="expected"/> based upon <paramref name="comparer"/>.
        /// </summary>
        /// <typeparam name="T">
        ///   The type to deserialize the JSON into.
        /// </typeparam>
        /// <param name="expected">
        ///   The expected object.
        /// </param>
        /// <param name="comparer">
        ///   The comparer to determine the eqivalence of the expected and the actual.
        /// </param>
        /// <param name = "because">
        ///   A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "becauseArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "because" />.
        /// </param>
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

        /// <summary>
        ///   Asserts that the <see cref="HttpContent"/> contains JSON that matches the condition in <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">
        ///   A predicate to match the the json content against.
        /// </param>
        /// <param name = "because">
        ///   A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "becauseArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "because" />.
        /// </param>
        public AndConstraint<HttpContentAssertions> HaveJsonMatch(Expression<Func<JToken, bool>> predicate, string because = "", params object[] becauseArgs)
        {
            var jToken = GetJTokenContent(because, becauseArgs, ExpectedContentMatch, predicate.Body);

            Execute.Assertion
                .ForCondition(predicate.Compile()(jToken))
                .BecauseOf(because, becauseArgs)
                .FailWith(ExpectedContentMatch, predicate.Body);

            return new AndConstraint<HttpContentAssertions>(this);
        }

        /// <summary>
        ///   Asserts that the <see cref="HttpContent"/> contains JSON that when deserialized into <typeparamref name="T"/> 
        ///   matches the condition in <paramref name="predicate"/>
        /// </summary>
        /// <typeparam name="T">
        ///   The type to deserialize the JSON into.
        /// </typeparam>
        /// <param name="predicate">
        ///   A predicate to match the the json content against.
        /// </param>
        /// <param name = "because">
        ///   A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "becauseArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "because" />.
        /// </param>
        public AndConstraint<HttpContentAssertions> HaveJsonMatch<T>(Expression<Func<T, bool>> predicate, string because = "", params object[] becauseArgs)
        {
            var obj = GetJsonContent<T>(because, becauseArgs, ExpectedContentMatch, predicate.Body);

            Execute.Assertion
                .ForCondition(predicate.Compile()(obj))
                .BecauseOf(because, becauseArgs)
                .FailWith(ExpectedContentMatch, predicate.Body);

            return new AndConstraint<HttpContentAssertions>(this);
        }

        /// <summary>
        ///   Returns the type of the subject the assertion applies on.
        /// </summary>
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
                .ForCondition(IsMediaType(JsonMediaType, contentMediaType))
                .BecauseOf(because, becauseArgs)
                .FailWith(failString, failArgs.ToArray());
        }

        private static bool IsMediaType(string expected, string actual)
        {
            return string.Equals(expected, actual, StringComparison.Ordinal);
        }
    }
}
