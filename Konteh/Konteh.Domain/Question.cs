using Konteh.Domain.Enumeration;
using System.Net;

namespace Konteh.Domain
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<Answer> Answers { get; set; } = [];
        public QuestionCategory Category { get; set; }
        public QuestionType Type { get; set; }

        public void Edit(Question question, IEnumerable<Answer> answers)
        {
            Text = question.Text;
            Category = question.Category;
            Type = question.Type;

            for (int i = 0; i < Answers.Count; i++)
            {
                var updatedAnswer = question.Answers.Find(x => x.Id == Answers[i].Id);
                if (updatedAnswer != null)
                {
                    Answers[i].Edit(updatedAnswer);
                }
                else
                {
                    Answers.RemoveAt(i--);
                }
            }

            Answers.AddRange(answers);
        }
    }
}
