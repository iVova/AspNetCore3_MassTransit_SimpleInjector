using MassTransit;
using Microsoft.Extensions.Logging;
using MyAspCoreApp.Messages;
using MyAspCoreApp.Services;
using System.Threading.Tasks;

namespace MyAspCoreApp.Consumers
{
    public class SendNotificationOnUserCreatedConsumer : IConsumer<UserCreatedMessage>
    {
        private readonly IClock _clock;
        private readonly ILogger<SendNotificationOnUserCreatedConsumer> _logger;

        public SendNotificationOnUserCreatedConsumer(IClock clock, ILogger<SendNotificationOnUserCreatedConsumer> logger)
        {
            _clock = clock;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<UserCreatedMessage> context)
        {
            // Send email...
            _logger.LogInformation("Current time {currentTime}", _clock.UtcNow.DateTime);
            return Task.CompletedTask;
        }
    }
}
