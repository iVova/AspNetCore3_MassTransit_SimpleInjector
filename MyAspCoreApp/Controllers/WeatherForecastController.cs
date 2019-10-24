using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAspCoreApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace MyAspCoreApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IClock _clock;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IClock clock, ILogger<WeatherForecastController> logger)
        {
            _clock = clock;
            _logger = logger;
        }

        [HttpGet]
        public DateTime Get()
        {
            _logger.LogInformation("Hello world! Today: {TodayTime}", _clock.UtcNow.DateTime);
            //await _bus.Publish(new UserCreatedMessage
            //{
            //    UserId = Guid.NewGuid()
            //});

            return _clock.UtcNow.DateTime;
        }
    }
}
