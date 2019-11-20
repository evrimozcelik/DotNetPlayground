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

        public void Start()
        {
            _sw.Start();
        }

        public void Stop()
        {
            _sw.Stop();
        }

        public void LogElapsedTime(string action="")
        {
            var sourceClass = typeof(T);
            var elapsedTime = _sw.ElapsedMilliseconds;
            _logger.LogInformation("SourceClass: {SourceClass}, Action: {Action}, ElapsedTime: {ElapsedTime}", sourceClass, action, elapsedTime);
        }


        public void Dispose()
        {
            LogElapsedTime();
        }
    }
}
