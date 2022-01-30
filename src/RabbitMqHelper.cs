using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace dotnet_sensors_rabbit;

public class RabbitMqHelper
{
    private readonly ILogger<RabbitMqHelper> _logger;

    private readonly ServiceBindingsReader _serviceBindingsReader;

    public RabbitMqHelper(ILogger<RabbitMqHelper> logger, ServiceBindingsReader serviceBindingsReader)
    {
        _logger = logger;
        _serviceBindingsReader = serviceBindingsReader;
    }

    public void Send(SensorData sensorData)
    {
        using (var connection = GetRabbitMqConnection())
        using (var channel = connection.CreateModel())
        {
            //channel.ExchangeDeclare(exchange: "sensor-data", type: "topic");

            var options = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var jsonString = JsonSerializer.Serialize(sensorData, options);
            _logger.LogInformation("Published message: [{jsonString}]", jsonString);
            
            var body = Encoding.UTF8.GetBytes(jsonString);

            var props = channel.CreateBasicProperties();
            props.DeliveryMode = 1; // Non-Persistent
            
            channel.BasicPublish("sensor-data",
                "baz.bam",
                props,
                body);
        }
    }
    
    private IConnection GetRabbitMqConnection()
    {
        const string serviceName = "rmq";
        var dict = _serviceBindingsReader.Get(serviceName);

        var host = dict["host"];
        var port = Convert.ToInt32(dict["port"]);
        var user = dict["username"];
        var password = dict["password"];

        _logger.LogInformation(
            "RabbitMQ service [{name}], host [{host}], port [{port}], user [{user}], password: [{password}]",
            serviceName, host, port, user, password);

        var factory = new ConnectionFactory
        {
            HostName = host, UserName = user, Password = password, Port = port
        };

        return factory.CreateConnection();
    }
}

public class SensorData
{
    public Guid Id { get; set; }
    public double Temperature { get; set; }
    public double Pressure { get; set; }
}