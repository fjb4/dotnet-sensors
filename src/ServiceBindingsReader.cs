namespace dotnet_sensors_rabbit;

public class ServiceBindingsReader
{
    private readonly ILogger<ServiceBindingsReader> _logger;

    public ServiceBindingsReader(ILogger<ServiceBindingsReader> logger)
    {
        _logger = logger;
    }

    public IDictionary<string, string> Get(string bindingName)
    {
        var serviceBindingRoot = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT");
        _logger.LogInformation("Service Binding Root: {root}", serviceBindingRoot);

        return !string.IsNullOrEmpty(serviceBindingRoot)
            ? ReadFiles(serviceBindingRoot, bindingName)
            : new Dictionary<string, string>();
    }

    private IDictionary<string, string> ReadFiles(string root, string name)
    {
        var dict = new Dictionary<string, string>();
        var folder = Path.Combine(root, name);

        if (Directory.Exists(folder))
        {
            var files = Directory.GetFiles(folder);

            foreach (var file in files)
            {
                var key = Path.GetFileName(file);
                var value = File.ReadAllText(file);
                _logger.LogInformation("Service binding {key}: {value}", file, value);

                dict.Add(key, value);
            }
        }

        return dict;
    }
}