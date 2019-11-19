using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PerformanceLogging
{
    public class AnalysisMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _middlewareName;

        private IPerformanceLogger<AnalysisMiddleware> _performanceLogger;

        public AnalysisMiddleware(RequestDelegate next, string middlewareName, IPerformanceLogger<AnalysisMiddleware> performanceLogger)
        {
            _next = next;
            if (string.IsNullOrEmpty(middlewareName))
            {
                middlewareName = next.Target.GetType().FullName;
            }
            _middlewareName = middlewareName;
            _performanceLogger = performanceLogger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var timer = _performanceLogger.StartTimer();

            try
            {
                await _next(httpContext);

                timer.LogElapsedTime(_middlewareName);
            }
            catch (Exception ex)
            {
                timer.LogElapsedTime($"{_middlewareName} exception");
                throw;
            }
        }
    }
}
