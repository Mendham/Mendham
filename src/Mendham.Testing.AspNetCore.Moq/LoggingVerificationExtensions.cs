using Moq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
