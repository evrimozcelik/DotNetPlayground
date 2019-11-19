using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace PerformanceLogging
{
    public class PerformanceLogger<T> : IPerformanceLogger<T>
    {
        private ILogger<PerformanceLogger<T>> _logger;

        public PerformanceLogger(ILogger<PerformanceLogger<T>> logger)
        {
            _logger = logger;
        }

        public Timer<T> StartTimer()
        {
            return new Timer<T>(_logger);
        }
    }

    public class Timer<T> : IDisposable
    {
        private Stopwatch _sw;
        private ILogger _logger;

        public Timer(ILogger logger)
        {
            _sw = Stopwatch.StartNew();
            _logger = logger;
        }

        public void LogElapsedTime(string action="")
        {
            _logger.LogInformation($"Class: {typeof(T)}, Action: {action}, ElapsedTime: {_sw.ElapsedMilliseconds}");
        }


        public void Dispose()
        {
            LogElapsedTime();
        }
    }
}
