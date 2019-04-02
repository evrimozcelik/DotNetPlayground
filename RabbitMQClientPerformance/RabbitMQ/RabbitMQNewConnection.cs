using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQClientPerformance.RabbitMQ
{
    public class RabbitMQNewConnection : IRabbitMQClient
    {

        public string Post(MyData data)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //channel.QueueDeclare(queue: "MyQueue",
                                     //durable: false,
                                     //exclusive: false,
                                     //autoDelete: false,
                                     //arguments: null);

                var serializedData = JsonConvert.SerializeObject(data);
                var body = Encoding.UTF8.GetBytes(serializedData);

                channel.BasicPublish(exchange: "",
                                     routingKey: "MyQueue",
                                     basicProperties: null,
                                     body: body);
                
                return $"[x] Sent {data.ID}";
            }
        }
    }
}
