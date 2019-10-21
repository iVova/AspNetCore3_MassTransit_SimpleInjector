using MassTransit;
using MyAspCoreApp.Messages;
using System.Threading.Tasks;

namespace MyAspCoreApp.Consumers
{
    public class SendNotificationOnUserCreatedConsumer : IConsumer<UserCreatedMessage>
    {
        public SendNotificationOnUserCreatedConsumer()
        {
        }

        public Task Consume(ConsumeContext<UserCreatedMessage> context)
        {
            // Send email...
            return Task.CompletedTask;
        }
    }
}
