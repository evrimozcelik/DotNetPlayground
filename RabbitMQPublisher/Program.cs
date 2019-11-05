using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;

namespace RabbitMQPublisher
{
    public class Program
    {
        public static IMetricsRoot Metrics;
        public static CounterOptions SuccessCounter = new CounterOptions { Name = "success-counter" };
        public static CounterOptions FailCounter = new CounterOptions { Name = "fail-counter" };
        public static MeterOptions Rate = new MeterOptions { Name = "rate", MeasurementUnit= Unit.Calls, RateUnit=TimeUnit.Seconds};
        public static HistogramOptions Time = new HistogramOptions { Name = "time" };


        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            Metrics = new MetricsBuilder()
                .Report.ToConsole()
                .Build();

            var cancellationTokenSource = new CancellationTokenSource();

            Log.Information("RabbitMQPublisher started!");

            for (int i=0;i<50;i++)
            {
                var name = $"Publisher{i}";
                var publisher = new Publisher(name, cancellationTokenSource.Token);
                Task.Run((Action)publisher.Publish, cancellationTokenSource.Token);

                Log.Information($"Publisher {name} started!");
            }

            var input = String.Empty;

            while(input != "q")
            {
                Log.Information("Press 'q' to stop");
                var key = Console.ReadKey();
                input = key.KeyChar.ToString();
            }

            cancellationTokenSource.Cancel();

            Task.WhenAll(Metrics.ReportRunner.RunAllAsync());

            Log.Information("Press key to exit");
            Console.ReadKey();

        }

    }


    public class Publisher
    {
        public string _name { get; }
        private readonly CancellationToken _cancellationToken;

        public Publisher(string name, CancellationToken cancellationToken)
        {
            _name = name;
            _cancellationToken = cancellationToken;

        }

        public void Publish()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {

                var counter = 0;

                while (!_cancellationToken.IsCancellationRequested)
                {

                    using (var channel = connection.CreateModel())
                    {
                        channel.ConfirmSelect();

                        //Thread.Sleep(10);

                        Stopwatch stopWatch = Stopwatch.StartNew();

                        var message = $"{_name}-{counter}";

                        var serializedData = JsonConvert.SerializeObject(message);
                        var body = Encoding.UTF8.GetBytes(serializedData);

                        IBasicProperties properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        properties.DeliveryMode = 2;

                        channel.BasicPublish(exchange: "AuditExchange",
                                             routingKey: "AuditLog",
                                             mandatory: true,
                                             basicProperties: properties,
                                             body: body);

                        var success = channel.WaitForConfirms(TimeSpan.FromMilliseconds(100));

                        stopWatch.Stop();

                        Log.Information($"Sent {message}");

                        if (success)
                        {
                            Program.Metrics.Measure.Counter.Increment(Program.SuccessCounter);
                        }
                        else
                        {
                            Program.Metrics.Measure.Counter.Increment(Program.FailCounter);
                        }

                        Program.Metrics.Measure.Meter.Mark(Program.Rate);
                        Program.Metrics.Measure.Histogram.Update(Program.Time, stopWatch.ElapsedMilliseconds);

                    }

                    counter++;

                }
            }
        }


    }
}
