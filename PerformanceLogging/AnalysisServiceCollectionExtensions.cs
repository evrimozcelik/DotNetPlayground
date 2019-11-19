using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PerformanceLogging
{
    /// <summary>
    /// Extension methods for setting up diagnostic services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class AnalysisServiceCollectionExtensions
    {
        /// <summary>
        /// Adds diagnostic services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddMiddlewareAnalysis(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Prevent registering the same implementation of IStartupFilter (AnalysisStartupFilter) multiple times.
            // But allow multiple registrations of different implementation types.
            services.TryAddEnumerable(ServiceDescriptor.Transient<IStartupFilter, AnalysisStartupFilter>());
            return services;
        }
    }
}
