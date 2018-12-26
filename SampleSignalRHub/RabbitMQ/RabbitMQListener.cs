using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SampleRabbitMQData;

namespace SampleSignalRHub.RabbitMQ
{
	public class RabbitMQListener : IHostedService
    {
        //static HubConnection connectionSignalR;


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/notificationhub")
                .Build();

            await hubConnection.StartAsync();

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //Trigger The SignalR
                //Connect().Wait();

                channel.QueueDeclare(queue: "N1",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body;
                    var data = Encoding.UTF8.GetString(body);
                    Notification notification = JsonConvert.DeserializeObject<Notification>(data);
                    Console.WriteLine(" [x] Received {0}", notification.Name + " : " + notification.Value);

                    //hubProxy.Invoke("PushNotify", notification);

                    await hubConnection.InvokeAsync("PushNotify", notification);

                    //connectionSignalR.InvokeAsync("PushNotify", notification);
                    //-------------------------

                };
                channel.BasicConsume(queue: "N1",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }


            return;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


    }
}
