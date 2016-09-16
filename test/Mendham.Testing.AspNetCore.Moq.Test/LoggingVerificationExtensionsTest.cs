using Microsoft.Extensions.Logging;
using Mendham.Testing.Moq;
using Moq;
using System;
using Xunit;
using FluentAssertions;

namespace Mendham.Testing.AspNetCore.Moq.Test
{
    public class LoggingVerificationExtensionsTest
    {
        private const string FailMessage = "the log verification failed";

        [Fact]
        public void VerifyLogFullWithFunc_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, Times.Once, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogFullWithFunc_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => true, ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogFullWithFunc_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogFullWithFunc_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Error, a => a.ToString().Contains(msg),
                ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogFullWithFunc_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => false,
                ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogFullWithTimes_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, Times.Once(), FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogFullWithTimes_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => true, ex, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogFullWithTimes_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogFullWithTimes_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Error, a => a.ToString().Contains(msg),
                ex, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogFullWithTimes_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => false,
                ex, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogFullWithFailMessageOnly_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogFullWithFailMessageOnly_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => true, ex, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogFullWithFailMessageOnly_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogFullWithFailMessageOnly_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Error, a => a.ToString().Contains(msg),
                ex, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogFullWithFailMessageOnly_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => false, ex, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogExceptionWithFunc_EntryWithException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, Times.Once, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogExceptionWithFunc_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogExceptionWithFunc_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogExceptionWithFunc_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Error, ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogExceptionWithTimes_EntryWithException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, Times.Once(), FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogExceptionWithTimes_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, ex, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogExceptionWithTimes_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, ex, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogExceptionWithTimes_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Error, ex, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogExceptionWithFailMessageOnly_EntryWithException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, ex, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogExceptionWithFailMessageOnly_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, ex, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogExceptionWithFailMessageOnly_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, ex, FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogExceptionWithFailMessageOnly_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Error, ex, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogByMessageWithFunc_EntryWithMessage_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => a.ToString().Contains("some error: message"),
                Times.Once, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogByMessageWithFunc_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => true, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogByMessageWithFunc_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Error, a => a.ToString().Contains("some error: message"),
                Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogByMessageWithFunc_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => false,
                Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogByMessageWithTimes_EntryWithMessage_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => a.ToString().Contains("some error: message"),
                Times.Once(), FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogByMessageWithTimes_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => true, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogByMessageWithTimes_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Error, a => a.ToString().Contains("some error: message"),
                Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogByMessageWithTimes_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => false,
                Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogByMessageWithFailMessageOnly_EntryWithMessage_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => a.ToString().Contains("some error: message"),
                FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogByMessageWithFailMessageOnly_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => true, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogByMessageWithFailMessageOnly_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Error, a => a.ToString().Contains("some error: message"),
                FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogByMessageWithFailMessageOnly_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, a => false, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogLogLevelWithFunc_EntryCalled_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, Times.Once, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogLogLevelWithFunc_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogLogLevelWithFunc_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Error, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogLogLevelWithTimes_EntryCalled_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, Times.Once(), FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogLogLevelWithTimes_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogLogLevelWithTimes_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Error, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogLogLevelWithFailMessageOnly_EntryCalled_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning,
                FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogLogLevelWithFailMessageOnly_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Warning, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogLogLevelWithFailMessageOnly_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLog(LogLevel.Error, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessagePredicateWithFunc_EntryWithMessage_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, a => a.Contains("some error: message"),
                Times.Once, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogMessagePredicateWithFunc_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, a => true, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessagePredicateWithFunc_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Error, a => a.Contains("some error: message"),
                Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessagePredicateWithFunc_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, a => false,
                Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessagePredicateWithTimes_EntryWithMessage_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, a => a.Contains("some error: message"),
                Times.Once(), FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogMessagePredicateWithTimes_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, a => true, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessagePredicateWithTimes_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Error, a => a.Contains("some error: message"),
                Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VVerifyLogMessagePredicateWithTimes_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, a => false,
                Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessagePredicateWithFailMessageOnly_EntryWithMessage_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, a => a.Contains("some error: message"),
                FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogMessagePredicateWithFailMessageOnly_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, a => true, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessagePredicateWithFailMessageOnly_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Error, a => a.Contains("some error: message"),
                FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessagePredicateWithFailMessageOnly_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, a => false, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessageStringWithFunc_EntryWithMessage_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, "some error: message",
                Times.Once, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogMessageStringWithFunc_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, "some error: message", 
                Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessageStringWithFunc_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Error, "some error: message",
                Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessageStringWithFunc_MessageNotEqual_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, "wrong message",
                Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessageStringWithTimes_EntryWithMessage_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, "some error: message",
                Times.Once(), FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogMessageStringWithTimes_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, "some error: message",
                Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessageStringWithTimes_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Error, "some error: message",
                Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessageStringWithTimes_MessageNotEqual_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, "wrong message",
                Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessageStringWithFailMessageOnly_EntryWithMessage_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, "some error: message",
                FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogMessageStringWithFailMessageOnly_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, "some error: message", FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessageStringWithFailMessageOnly_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Error, "some error: message",
                FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogMessageStringWithFailMessageOnly_MessageNotEqual_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogMessage(LogLevel.Warning, "wrong message", FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyErrorFull_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogError(0, ex, msg);

            Action act = () => logger.AsMock().VerifyError(a => a.ToString().Contains(msg),
                ex, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyErrorFull_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyError(a => true, ex, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyErrorFull_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogError(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyError(a => a.ToString().Contains(msg),
                ex, FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyErrorFull_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyError(a => a.ToString().Contains(msg),
                ex, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyErrorFull_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogError(0, ex, msg);

            Action act = () => logger.AsMock().VerifyError(a => false,
                ex, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyErrorByLogValues_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogError(msg, "message");

            Action act = () => logger.AsMock().VerifyError(a => a.ToString().Contains("some error: message"),
                FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyErrorByLogValues_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyError(a => true, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyErrorByLogValues_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyError(a => a.ToString().Contains(msg), FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyErrorByLogValues_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogError(msg, "message");

            Action act = () => logger.AsMock().VerifyError(a => false, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyErrorException_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogError(0, ex, msg);

            Action act = () => logger.AsMock().VerifyError(ex, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyErrorException_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyError(ex, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyErrorException_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogError(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyError(ex, FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyErrorException_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyError(ex, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyErrorByMessage_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogError(msg, "message");

            Action act = () => logger.AsMock().VerifyError("some error: message", FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyErrorByMessage_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyError("message", FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyErrorByMessage_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyError("some error: message", FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyErrorByMessage_MessageDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogError(msg, "message");

            Action act = () => logger.AsMock().VerifyError("wrong message", FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyWarningFull_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyWarning(a => a.ToString().Contains(msg),
                ex, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyWarningFull_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyWarning(a => true, ex, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyWarningFull_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyWarning(a => a.ToString().Contains(msg),
                ex, FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyWarningFull_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some message";

            logger.LogError(0, ex, msg);

            Action act = () => logger.AsMock().VerifyWarning(a => a.ToString().Contains(msg),
                ex, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyWarningFull_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyWarning(a => false,
                ex, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyWarningByLogValues_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some warning: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyWarning(a => a.ToString().Contains("some warning: message"),
                FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyWarningByLogValues_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyWarning(a => true, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyWarningByLogValues_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some warning: {0}";

            logger.LogError(msg, "message");

            Action act = () => logger.AsMock().VerifyWarning(a => a.ToString().Contains(msg), FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyWarningByLogValues_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some warning: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyWarning(a => false, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyWarningException_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyWarning(ex, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyWarningException_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyWarning(ex, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyWarningException_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyWarning(ex, FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyWarningException_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some message";

            logger.LogError(0, ex, msg);

            Action act = () => logger.AsMock().VerifyWarning(ex, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyWarningByMessage_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some warning: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyWarning("some warning: message", FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyWarningByMessage_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyWarning("message", FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyWarningByMessage_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some warning: {0}";

            logger.LogError(msg, "message");

            Action act = () => logger.AsMock().VerifyWarning("some warning: message", FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyWarningByMessage_MessageDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some warning: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyWarning("wrong message", FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyInformationByLogValues_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some information: {0}";

            logger.LogInformation(msg, "message");

            Action act = () => logger.AsMock().VerifyInformation(a => a.ToString().Contains("some information: message"),
                FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyInformationByLogValues_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyInformation(a => true, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyInformationByLogValues_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some information: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyInformation(a => a.ToString().Contains(msg), FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyInformationByLogValues_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some information: {0}";

            logger.LogInformation(msg, "message");

            Action act = () => logger.AsMock().VerifyInformation(a => false, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyInformationByMessage_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some information: {0}";

            logger.LogInformation(msg, "message");

            Action act = () => logger.AsMock().VerifyInformation("some information: message", FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyInformationByMessage_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyInformation("message", FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyInformationByMessage_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some information: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyInformation("some information: message", FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyInformationByMessage_MessageDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some information: {0}";

            logger.LogInformation(msg, "message");

            Action act = () => logger.AsMock().VerifyInformation("wrong message", FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }
    }
}
