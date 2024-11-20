namespace Konteh.Domain
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;

        public void Edit(Answer updatedAnswer)
        {
            Text = updatedAnswer.Text;
            IsCorrect = updatedAnswer.IsCorrect;
        }
    }
}
