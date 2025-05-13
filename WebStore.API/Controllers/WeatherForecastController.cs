using Microsoft.AspNetCore.Mvc;
using WebStore.API.Services;

namespace WebStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherForecastService _weatherforecastService;
            
    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherforecastService)
    {
        _logger = logger;
        _weatherforecastService = weatherforecastService;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        var result = _weatherforecastService.Get();

        return result;
    }
}
