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
        public void VerifyLogEntryFullWithFunc_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, Times.Once, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogEntryFullWithFunc_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => true, ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryFullWithFunc_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryFullWithFunc_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Error, a => a.ToString().Contains(msg),
                ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryFullWithFunc_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => false,
                ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryFullWithTimes_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, Times.Once(), FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogEntryFullWithTimes_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => true, ex, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryFullWithTimes_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryFullWithTimes_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Error, a => a.ToString().Contains(msg),
                ex, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryFullWithFailMessageOnly_EntryWithMessageAndException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogEntryFullWithFailMessageOnly_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => true, ex, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryFullWithFailMessageOnly_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryFullWithFailMessageOnly_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Error, a => a.ToString().Contains(msg),
                ex, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryExceptionWithFunc_EntryWithException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, Times.Once, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogEntryExceptionWithFunc_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryExceptionWithFunc_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryExceptionWithFunc_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Error, ex, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryExceptionWithTimes_EntryWithException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => a.ToString().Contains(msg),
                ex, Times.Once(), FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogEntryExceptionWithTimes_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, ex, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryExceptionWithTimes_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, ex, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryExceptionWithTimes_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Error, ex, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryExceptionWithFailMessageOnly_EntryWithException_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, ex, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogEntryExceptionWithFailMessageOnly_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, ex, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryExceptionWithFailMessageOnly_ExceptionDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, new InvalidOperationException("alt exception"), msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, ex, FailMessage);

            act.ShouldThrow<MockException>("the exception did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryExceptionWithFailMessageOnly_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error message";

            logger.LogWarning(0, ex, msg);

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Error, ex, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryMessageWithFunc_EntryWithMessage_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => a.ToString().Contains("some error: message"),
                Times.Once, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogEntryMessageWithFunc_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => true, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryMessageWithFunc_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Error, a => a.ToString().Contains("some error: message"),
                Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryMessageWithFunc_FormattedLogValuesPredicateFalse_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => false,
                Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the formatted log levels predicate is false")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryMessageWithTimes_EntryWithMessage_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => a.ToString().Contains("some error: message"),
                Times.Once(), FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogEntryMessageWithTimes_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => true, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryMessageWithTimes_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Error, a => a.ToString().Contains("some error: message"),
                Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryMessageWithFailMessageOnly_EntryWithMessage_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => a.ToString().Contains("some error: message"),
                FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogEntryMessageWithFailMessageOnly_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var ex = new InvalidOperationException("error");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, a => true, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryMessageWithFailMessageOnly_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Error, a => a.ToString().Contains("some error: message"),
                FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryLogLevelWithFunc_EntryCalled_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, Times.Once, FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogEntryLogLevelWithFunc_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryLogLevelWithFunc_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Error, Times.Once, FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryLogLevelWithTimes_EntryCalled_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, Times.Once(), FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogEntryLogLevelWithTimes_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryLogLevelWithTimes_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Error, Times.Once(), FailMessage);

            act.ShouldThrow<MockException>("the log level did not match")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryLogLevelWithFailMessageOnly_EntryCalled_DoesNotThrow()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning,
                FailMessage);

            act.ShouldNotThrow();
        }

        [Fact]
        public void VerifyLogEntryLogLevelWithFailMessageOnly_NoLogEntry_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Warning, FailMessage);

            act.ShouldThrow<MockException>("the logger was never called")
                .Where(a => a.Message.StartsWith(FailMessage), "that is the fail message");
        }

        [Fact]
        public void VerifyLogEntryLogLevelWithFailMessageOnly_LogLevelDoesNotMatch_Throws()
        {
            var logger = Mock.Of<ILogger<LoggingVerificationExtensionsTest>>();
            var msg = "some error: {0}";

            logger.LogWarning(msg, "message");

            Action act = () => logger.AsMock().VerifyLogEntry(LogLevel.Error, FailMessage);

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
        public void VVerifyLogMessageStringWithTimes_MessageNotEqual_Throws()
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
    }
}
