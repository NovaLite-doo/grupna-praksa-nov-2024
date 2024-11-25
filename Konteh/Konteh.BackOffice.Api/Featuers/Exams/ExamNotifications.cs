using Konteh.Domain.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Konteh.BackOffice.Api.Featuers.Exams
{
    public class ExamNotifications
    {
        public sealed class NotificationHub : Hub
        {
        }

        public class NotificationConsumer : IConsumer<ExamEvent>
        {
            private readonly IHubContext<NotificationHub> _hub;

            public NotificationConsumer(IHubContext<NotificationHub> hub)
            {
                _hub = hub;
            }

            public async Task Consume(ConsumeContext<ExamEvent> context)
            {
                var message = context.Message;

                await _hub.Clients.All.SendAsync("ReceiveNotification", message);

                return;
            }
        }
    }
}
