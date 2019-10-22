using MassTransit;
using MyAspCoreApp.Messages;
using System.Threading.Tasks;
using MyAspCoreApp.Services;

namespace MyAspCoreApp.Consumers
{
    public class SendNotificationOnUserCreatedConsumer : IConsumer<UserCreatedMessage>
    {
        private readonly IClock _clock;

        public SendNotificationOnUserCreatedConsumer(IClock clock)
        {
            _clock = clock;
        }

        public Task Consume(ConsumeContext<UserCreatedMessage> context)
        {
            // Send email...

            var time = _clock.UtcNow;
            return Task.CompletedTask;
        }
    }
}
