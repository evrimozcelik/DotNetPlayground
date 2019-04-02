using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQClientPerformance.RabbitMQ
{
    public class RabbitMQThreadLocal : IRabbitMQClient
    {
        private IConnection connection;
        private ThreadLocal<IModel> channelVar;

        private ConcurrentDictionary<int, IModel> channelMap;

        public RabbitMQThreadLocal() {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            connection = connectionFactory.CreateConnection();

            channelMap = new ConcurrentDictionary<int, IModel>();
        }

        public string Post(MyData data)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            IModel channel;
            if(channelMap.ContainsKey(id))
            {
                channelMap.TryGetValue(id, out channel);
            }
            else 
            {
                channel = connection.CreateModel();
                channelMap.TryAdd(id, channel);

                Console.WriteLine($"RabbitMQ Channel created for {id}");
            }

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
