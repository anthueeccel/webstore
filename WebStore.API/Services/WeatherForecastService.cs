using WebStore.API.Controllers;

namespace WebStore.API.Services
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get();
        IEnumerable<WeatherForecast> Create(int totalResults, int minTemp, int maxTemp);
    }

    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly ILogger<WeatherForecastController> _logger;

        private static readonly string[] Summaries =
        {
               "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastService(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        public WeatherForecastService()
        {
        }

        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-10, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        public IEnumerable<WeatherForecast> Create(int totalResults, int minTemp = -10, int maxTemp = 55)
        {
            return Enumerable.Range(1, totalResults).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(minTemp, maxTemp),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
