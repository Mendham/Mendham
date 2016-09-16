using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using System;
using System.Linq.Expressions;

namespace Mendham.Testing.Moq
{
    public static class LoggingVerificationExtensions
    {
        public static void VerifyLog<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<FormattedLogValues, bool>> formattedLogValuesPredicate, Exception exception, Times times,
            string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLog(loggerMock, logLevel, formattedLogValuesPredicate, exception, () => times, failMessage);
        }

        public static void VerifyLog<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<FormattedLogValues, bool>> formattedLogValuesPredicate, Exception exception,
            Func<Times> times, string failMessage = null) where TLogger : class, ILogger
        {
            loggerMock.Verify(a => a.Log(logLevel, It.IsAny<EventId>(),  It.Is(formattedLogValuesPredicate), 
                exception, It.IsAny<Func<object, Exception, string>>()), times, failMessage);
        }

        public static void VerifyLog<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<FormattedLogValues, bool>> formattedLogValuesPredicate, Exception exception, 
            string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLog(loggerMock, logLevel, exception, Times.Once, failMessage);
        }

        public static void VerifyLog<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Exception exception, Times times, string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLog(loggerMock, logLevel, exception, () => times, failMessage);
        }

        public static void VerifyLog<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Exception exception, Func<Times> times, string failMessage = null)
            where TLogger : class, ILogger
        {
            loggerMock.Verify(a => a.Log(logLevel, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(),
                exception, It.IsAny<Func<object, Exception, string>>()), times, failMessage);
        }

        public static void VerifyLog<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Exception exception, string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLog(loggerMock, logLevel, exception, Times.Once, failMessage);
        }

        public static void VerifyLog<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<FormattedLogValues, bool>> formattedLogValuesPredicate, Times times, 
            string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLog(loggerMock, logLevel, formattedLogValuesPredicate, () => times, failMessage);
        }

        public static void VerifyLog<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<FormattedLogValues, bool>> formattedLogValuesPredicate, Func<Times> times, 
            string failMessage = null) where TLogger : class, ILogger
        {
            loggerMock.Verify(a => a.Log(logLevel, It.IsAny<EventId>(), It.Is(formattedLogValuesPredicate),
                It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), times, failMessage);
        }

        public static void VerifyLog<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Expression<Func<FormattedLogValues, bool>> formattedLogValuesPredicate, string failMessage = null)
            where TLogger : class, ILogger
        {
            VerifyLog(loggerMock, logLevel, formattedLogValuesPredicate, Times.Once, failMessage);
        }

        public static void VerifyLog<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Times times, string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLog(loggerMock, logLevel, () => times, failMessage);
        }

        public static void VerifyLog<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            Func<Times> times, string failMessage = null)
            where TLogger : class, ILogger
        {
            loggerMock.Verify(a => a.Log(logLevel, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(),
                It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), times, failMessage);
        }

        public static void VerifyLog<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            string failMessage = null)
            where TLogger : class, ILogger
        {
            VerifyLog(loggerMock, logLevel, Times.Once, failMessage);
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

        public static void VerifyLogMessage<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            string message, Times times, string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLogMessage(loggerMock, logLevel, message, () => times, failMessage);
        }

        public static void VerifyLogMessage<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            string message, Func<Times> times, string failMessage = null) where TLogger : class, ILogger
        {
            loggerMock.Verify(a => a.Log(logLevel, It.IsAny<EventId>(), 
                It.Is<FormattedLogValues>(flv => string.Equals(flv.ToString(), message)),
                It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), times, failMessage);
        }

        public static void VerifyLogMessage<TLogger>(this Mock<TLogger> loggerMock, LogLevel logLevel,
            string message, string failMessage = null) where TLogger : class, ILogger
        {
            VerifyLogMessage(loggerMock, logLevel, message, Times.Once, failMessage);
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
