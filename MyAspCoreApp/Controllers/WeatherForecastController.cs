using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAspCoreApp.Messages;
using MyAspCoreApp.Services;
using System;
using System.Threading.Tasks;

namespace MyAspCoreApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IClock _clock;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IBus bus, IClock clock, ILogger<WeatherForecastController> logger)
        {
            _bus = bus;
            _clock = clock;
            _logger = logger;
        }

        [HttpGet]
        public async Task<DateTime> Get()
        {
            _logger.LogInformation("Hello world! Today: {TodayTime}", _clock.UtcNow.DateTime);
            await _bus.Publish(new UserCreatedMessage
            {
                UserId = Guid.NewGuid()
            });

            return _clock.UtcNow.DateTime;
        }
    }
}
