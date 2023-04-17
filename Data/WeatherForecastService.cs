using System.Diagnostics.CodeAnalysis;

namespace YTCmtyParser.Data;

public class WeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "冷凍", "寒冷伴風", "寒冷", "涼爽", "微溫", "溫暖", "適溫", "熱", "悶熱", "灼熱"
    };

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<暫止>")]
    public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
    {
        return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray());
    }
}