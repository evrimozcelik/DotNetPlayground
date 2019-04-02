using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQClientPerformance.RabbitMQ
{
    public class RabbitMQNewChannel : IRabbitMQClient
    {
        private IConnection connection;

        public RabbitMQNewChannel() {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            connection = connectionFactory.CreateConnection();
        }

        public string Post(MyData data)
        {
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
