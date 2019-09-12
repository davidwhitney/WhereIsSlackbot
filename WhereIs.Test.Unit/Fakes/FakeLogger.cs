using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace WhereIs.Test.Unit.Fakes
{
    [TestFixture]
    public class FakeLoggerTests
    {
        [Test]
        public void DoesntCrash()
        {
            var sut = new FakeLogger();

            sut.LogError("Something");

            Assert.That(sut.Entries.Count, Is.EqualTo(1));
            Assert.That(sut.IsEnabled(LogLevel.Error), Is.True);
            Assert.Throws<NotImplementedException>(() => sut.BeginScope(null));
        }
    }

    public class FakeLogger : ILogger
    {
        public IList<string> Entries { get; } = new List<string>();
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => Entries.Add(formatter(state, exception));
        public bool IsEnabled(LogLevel logLevel) => true;
        public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
    }
}