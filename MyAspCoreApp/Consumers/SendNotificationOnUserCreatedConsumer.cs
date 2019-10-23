using MassTransit;
using MyAspCoreApp.Messages;
using MyAspCoreApp.Services;
using System.Threading.Tasks;

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
            var time = _clock.UtcNow;
            return Task.CompletedTask;
        }
    }
}
