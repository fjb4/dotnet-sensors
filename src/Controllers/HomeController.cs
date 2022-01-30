using System.Diagnostics;
using dotnet_sensors_rabbit.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_sensors_rabbit.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SensorHostedService _sensorHostedService;

    public HomeController(ILogger<HomeController> logger, SensorHostedService sensorHostedService)
    {
        _logger = logger;
        _sensorHostedService = sensorHostedService;
    }

    public IActionResult Index()
    {
        var sensorIds = _sensorHostedService.GetSensorIds();
        return View(sensorIds);
    }

    [Route("Start")]
    public IActionResult Start()
    {
        _sensorHostedService.StartAsync(CancellationToken.None);
        return View();
    }

    [Route("Stop")]
    public IActionResult Stop()
    {
        _sensorHostedService.StopAsync(CancellationToken.None);
        return View();
    }

    [Route("AddSensor")]
    public IActionResult AddSensor()
    {
        _sensorHostedService.AddSensor();

        var sensorIds = _sensorHostedService.GetSensorIds();
        return View(sensorIds.Count());
    }

    [Route("RemoveSensor")]
    public IActionResult RemoveSensor()
    {
        _sensorHostedService.RemoveSensor();

        var sensorIds = _sensorHostedService.GetSensorIds();
        return View(sensorIds.Count());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}