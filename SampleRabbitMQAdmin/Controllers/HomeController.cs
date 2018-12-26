using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleRabbitMQAdmin.Models;
using SampleRabbitMQAdmin.RabbitMQ;
using SampleRabbitMQData;

namespace SampleRabbitMQAdmin.Controllers
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
            var data = GetDummyData();
            return View(data);
        }

        [HttpPost]
        public IActionResult Push(Notification notification) 
        {
            UpdateDummyList(notification);
            RabbitMQPost rabbitMq=new RabbitMQPost(notification);
            Console.WriteLine(rabbitMq.Post());
            return RedirectToAction("Index");       
        }

        public List<Notification> GetDummyData()
        {
            return notificationList;
        }

        public void UpdateDummyList(Notification notification)
        {
            int index = notificationList.FindIndex(st => st.ID == notification.ID);
            notificationList[index] = notification;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
