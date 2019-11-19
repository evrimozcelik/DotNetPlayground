using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PerformanceLogging.Models;

namespace PerformanceLogging.Controllers
{
    public class HomeController : Controller
    {

        private IPerformanceLogger<HomeController> _performanceLogger;

        public HomeController(IPerformanceLogger<HomeController> performanceLogger)
        {
            _performanceLogger = performanceLogger;
        }

        public IActionResult Index()
        {
            var timer = _performanceLogger.StartTimer();
            Thread.Sleep(2000);
            timer.LogElapsedTime();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
