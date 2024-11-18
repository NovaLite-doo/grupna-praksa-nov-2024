namespace Konteh.Domain
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        public void Validate()
        {
            if (string.IsNullOrEmpty(Text))
            {
                throw new ArgumentException($"{nameof(Answer)} {nameof(Text)} is mandatory");
            }
        }

        public void Edit(Answer answer)
        {
            Text = answer.Text;
            IsCorrect = answer.IsCorrect;
        }
    }
}
