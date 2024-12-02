using Konteh.Domain.Events;

namespace Konteh.BackOffice.Api.Featuers.Exams.ExamNotifications
{
    public interface IExamNotificationClient
    {
        Task ReceiveNotification(SearchExams.ExamResponse exam);
    }
}
