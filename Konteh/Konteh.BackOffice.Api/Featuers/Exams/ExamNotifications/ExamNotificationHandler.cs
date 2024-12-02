using Konteh.Domain.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Konteh.BackOffice.Api.Featuers.Exams.ExamNotifications
{
    public class ExamNotificationHandler : IConsumer<ExamEvent>
    {
        private readonly IHubContext<ExamNotificationHub, IExamNotificationClient> _hub;

        public ExamNotificationHandler(IHubContext<ExamNotificationHub, IExamNotificationClient> hub)
        {
            _hub = hub;
        }

        public async Task Consume(ConsumeContext<ExamEvent> context)
        {
            var message = context.Message;

            var notification = new SearchExams.ExamResponse
            {
                Id = message.Id,
                Status = message.Status,
                Score = message.Score,
                Candidate = new SearchExams.CandidateResponse
                {
                    Name = message.Candidate.Name,
                    Surname = message.Candidate.Surname,
                }
            };



            await _hub.Clients.All.ReceiveNotification(notification);

            return;
        }
    }
}
