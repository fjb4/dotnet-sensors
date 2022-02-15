using System.Diagnostics;
using dotnet_sensors_rabbit.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_sensors_rabbit.Controllers;

public class HomeController : Controller
{
    private readonly SensorHostedService _sensorHostedService;

    public HomeController(SensorHostedService sensorHostedService)
    {
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

        var sensorIds = _sensorHostedService.GetSensorIds();
        return View(sensorIds);
    }

    [Route("Stop")]
    public IActionResult Stop()
    {
        _sensorHostedService.StopAsync(CancellationToken.None);

        var sensorIds = _sensorHostedService.GetSensorIds();
        return View(sensorIds);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}