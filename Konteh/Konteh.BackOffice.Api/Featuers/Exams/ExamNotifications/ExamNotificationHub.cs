using Microsoft.AspNetCore.SignalR;

namespace Konteh.BackOffice.Api.Featuers.Exams.ExamNotifications
{
    public sealed class ExamNotificationHub : Hub<IExamNotificationClient>
    {
        public const string Route = "/exam-notifications";
    }
}
