using System;
namespace PerformanceLogging
{
    public interface IPerformanceLogger<T>
    {
        Timer<T> StartTimer();
    }
}
