using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SemanticsWEB.Repositories;

namespace SemanticsWEB.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<SampleDataController> _logger;
        private readonly IDbPediaRepository _dbPediaRepository;

        public SampleDataController(ILogger<SampleDataController> logger, 
            IDbPediaRepository dbPediaRepository)
        {
            _logger = logger;
            _dbPediaRepository = dbPediaRepository;
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            _logger.LogInformation("i was here");
            //_dbPediaQueryService.doIt();
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get { return 32 + (int) (TemperatureC / 0.5556); }
            }
        }
    }
}