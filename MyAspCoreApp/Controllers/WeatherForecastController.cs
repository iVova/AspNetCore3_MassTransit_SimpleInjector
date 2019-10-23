using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MyAspCoreApp.Messages;
using System;
using System.Threading.Tasks;

namespace MyAspCoreApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IBus _bus;

        public WeatherForecastController(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public async Task Get()
        {
            var message = new UserCreatedMessage
            {
                UserId = Guid.NewGuid()
            };

            await _bus.Publish(message);
        }
    }
}
