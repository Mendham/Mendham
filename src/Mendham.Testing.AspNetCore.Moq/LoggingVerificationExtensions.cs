using Moq;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging.Internal;

namespace Mendham.Testing.Moq
{
    public static class LoggingVerificationExtensions
    {
        public static void VerifyLogEntry<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<FormattedLogValues, bool>> formattedLogValuesPredicate, Exception exception, Times times,
            string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLogEntry(loggerMock, logLevel, formattedLogValuesPredicate, exception, () => times, failMessage);
        }

        public static void VerifyLogEntry<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<FormattedLogValues, bool>> formattedLogValuesPredicate, Exception exception,
            Func<Times> times, string failMessage = null) where TLogger : class, ILogger
        {
            loggerMock.Verify(a => a.Log(logLevel, It.IsAny<EventId>(),  It.Is(formattedLogValuesPredicate), 
                exception, It.IsAny<Func<object, Exception, string>>()), times, failMessage);
        }

        public static void VerifyLogEntry<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<FormattedLogValues, bool>> formattedLogValuesPredicate, Exception exception, 
            string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLogEntry(loggerMock, logLevel, exception, Times.Once, failMessage);
        }

        public static void VerifyLogEntry<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Exception exception, Times times, string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLogEntry(loggerMock, logLevel, exception, () => times, failMessage);
        }

        public static void VerifyLogEntry<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Exception exception, Func<Times> times, string failMessage = null)
            where TLogger : class, ILogger
        {
            loggerMock.Verify(a => a.Log(logLevel, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(),
                exception, It.IsAny<Func<object, Exception, string>>()), times, failMessage);
        }

        public static void VerifyLogEntry<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Exception exception, string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLogEntry(loggerMock, logLevel, exception, Times.Once, failMessage);
        }

        public static void VerifyLogEntry<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<FormattedLogValues, bool>> formattedLogValuesPredicate, Times times, 
            string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLogEntry(loggerMock, logLevel, formattedLogValuesPredicate, () => times, failMessage);
        }

        public static void VerifyLogEntry<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<FormattedLogValues, bool>> formattedLogValuesPredicate, Func<Times> times, 
            string failMessage = null) where TLogger : class, ILogger
        {
            loggerMock.Verify(a => a.Log(logLevel, It.IsAny<EventId>(), It.Is(formattedLogValuesPredicate),
                It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), times, failMessage);
        }

        public static void VerifyLogEntry<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<FormattedLogValues, bool>> formattedLogValuesPredicate, string failMessage = null)
            where TLogger : class, ILogger
        {
            VerifyLogEntry(loggerMock, logLevel, formattedLogValuesPredicate, Times.Once, failMessage);
        }

        public static void VerifyLogEntry<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Times times, string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLogEntry(loggerMock, logLevel, () => times, failMessage);
        }

        public static void VerifyLogEntry<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Func<Times> times, string failMessage = null)
            where TLogger : class, ILogger
        {
            loggerMock.Verify(a => a.Log(logLevel, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(),
                It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), times, failMessage);
        }

        public static void VerifyLogEntry<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            string failMessage = null)
            where TLogger : class, ILogger
        {
            VerifyLogEntry(loggerMock, logLevel, Times.Once, failMessage);
        }

        public static void VerifyLogMessage<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<string, bool>> messagePredicate, Times times, string failMessage = null)
            where TLogger : class, ILogger
        {
            VerifyLogMessage(loggerMock, logLevel, messagePredicate, () => times, failMessage);
        }

        public static void VerifyLogMessage<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<string, bool>> messagePredicate, Func<Times> times, string failMessage = null)
            where TLogger : class, ILogger
        {
            Expression<Func<FormattedLogValues, string>> stringSelector = a => a.ToString();

            var expression = Expression.Lambda<Func<FormattedLogValues, bool>>(
                new SwapVistor(messagePredicate.Parameters[0], stringSelector.Body).Visit(messagePredicate.Body), 
                    stringSelector.Parameters);

            loggerMock.Verify(a => a.Log(logLevel, It.IsAny<EventId>(), It.Is(expression),
                It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), times, failMessage);
        }

        public static void VerifyLogMessage<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<string, bool>> messagePredicate, string failMessage = null)
            where TLogger : class, ILogger
        {
            VerifyLogMessage(loggerMock, logLevel, messagePredicate, Times.Once, failMessage);
        }

        private class SwapVistor : ExpressionVisitor
        {
            private readonly Expression _from;
            private readonly Expression _to;

            public SwapVistor(Expression from, Expression to)
            {
                _from = from;
                _to = to;
            }

            public override Expression Visit(Expression node)
            {
                return node == _from ? _to : base.Visit(node);
            }
        }
    }
}
