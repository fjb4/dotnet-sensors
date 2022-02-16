namespace dotnet_sensors_rabbit;

public class SensorHostedService : IHostedService, IDisposable
{
    private readonly ILogger<SensorHostedService> _logger;
    private readonly RabbitMqHelper _rabbitMqHelper;
    private readonly Random _random = new();
    private readonly Stack<Guid> _sensorIds = new();
    private Timer _timer = null!;

    public SensorHostedService(ILogger<SensorHostedService> logger, RabbitMqHelper rabbitMqHelper)
    {
        _logger = logger;
        _rabbitMqHelper = rabbitMqHelper;
        AddSensor(1);
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sensor Hosted Service started");

        _timer = new Timer(PublishSensorData, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sensor Hosted Service stopping");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private void PublishSensorData(object? state)
    {
        foreach (var sensorId in _sensorIds)
        {
            var sensorData = new SensorData
            {
                Id = sensorId,
                Temperature = _random.NextDouble() * 100,
                Pressure = _random.NextDouble() * 100
            };

            _rabbitMqHelper.Send(sensorData);
        }
    }

    public void AddSensor(int count = 1)
    {
        for (var i = 0; i < count; i++)
        {
            _sensorIds.Push(Guid.NewGuid());
        }
    }

    public void RemoveSensor()
    {
        _sensorIds.Pop();
    }

    public IEnumerable<Guid> GetSensorIds()
    {
        return _sensorIds;
    }
}