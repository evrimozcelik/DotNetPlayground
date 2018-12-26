using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleSignalRHub.Models;
using SampleRabbitMQData;

namespace SampleSignalRHub.Controllers
{
    public class HomeController : Controller
    {
        public static List<Notification> notificationList = new List<Notification>()
        {
            new Notification(){ID=1,Name="N1",Value=Decimal.Parse("1.0")},
            new Notification(){ID=2,Name="N2",Value=Decimal.Parse("2.0")},
            new Notification(){ID=3,Name="N3",Value=Decimal.Parse("3.0")}
        };

        public IActionResult Index()
        {
            return View(notificationList);
        }

        public IActionResult Detail(int ID) 
        {
            Notification notification = notificationList.FirstOrDefault(s => s.ID == ID);
            return View(notification);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
