using System;
namespace RabbitMQClientPerformance.RabbitMQ
{
    public interface IRabbitMQClient
    {
        string Post(MyData data);
    }
}
