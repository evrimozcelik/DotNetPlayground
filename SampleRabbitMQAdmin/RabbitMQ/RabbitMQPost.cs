using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SampleRabbitMQAdmin.Models;
using SampleRabbitMQData;

namespace SampleRabbitMQAdmin.RabbitMQ
{
    public class RabbitMQPost
    {
        public Notification data;
        public RabbitMQPost(Notification _data)
        {
            this.data = _data;
        }
        public string Post()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: data.Name,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var serializedData = JsonConvert.SerializeObject(data);
                var body = Encoding.UTF8.GetBytes(serializedData);

                channel.BasicPublish(exchange: "",
                                     routingKey: data.Name,
                                     basicProperties: null,
                                     body: body);
                
                return $"[x] Sent {data.Name}";
            }
        }
    }
}
