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
    [Route("/GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("Getting weatherforecast...");
        activity?.SetTag("WeatherForecast", "allWeather");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet]
    [Route("/CheckWeather")]
    public void CheckWeather()
    {
        DiagnosticsConfig.RequestCounter.Add(1,
            new("Action", nameof(Index)),
            new("Controller", nameof(WeatherForecastController)));

        System.Threading.Thread.Sleep(3500);
    }

}

