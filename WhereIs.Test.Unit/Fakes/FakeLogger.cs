using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace WhereIs.Test.Unit.Fakes
{
    public class FakeLogger : ILogger
    {
        public IList<string> Entries { get; } = new List<string>();
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => Entries.Add(formatter(state, exception));
        public bool IsEnabled(LogLevel logLevel) => true;
        public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
    }
}