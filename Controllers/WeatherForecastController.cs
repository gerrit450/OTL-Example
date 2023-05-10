using Microsoft.AspNetCore.Mvc;

namespace ServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

  
    [HttpGet]
    [Route("/GetWeatherForecast/${message}")]
    public IEnumerable<WeatherForecast> Get(string message)
    {
        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("Getting weatherforecast...");
        activity?.SetTag("Forecast mode", "fast");
        activity?.SetTag("Query parameter message, ", message ?? "no message!");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet]
    [Route("/TracingAndMetric")]
    public string TracingAndMetric()
    {
        DiagnosticsConfig.RequestCounter.Add(1,
            new("Action", nameof(Index)),
            new("Controller", nameof(WeatherForecastController)));

        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("Check Weather slow...");
        activity?.SetTag("Dummy id-", Guid.NewGuid().ToString());

        System.Threading.Thread.Sleep(2000);

        return "No weather found";
    }

    [HttpGet]
    [Route("/TelemetryPlusError")]
    public IActionResult TelemetryPlusError()
    {
        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("Give me error...");
        activity?.SetTag("Dummy id-", Guid.NewGuid().ToString());

        System.Threading.Thread.Sleep(3500);

        return BadRequest("Not really bad request!");
    }

}

