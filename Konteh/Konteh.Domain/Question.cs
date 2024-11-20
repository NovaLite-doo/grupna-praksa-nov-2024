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
        public bool IsDeleted { get; set; } = false;

        public void Edit(Question updatedQuestion, IEnumerable<Answer> newAnswers)
        {
            Text = updatedQuestion.Text;
            Category = updatedQuestion.Category;
            Type = updatedQuestion.Type;

            foreach (var answer in Answers)
            {
                var updatedAnswer = updatedQuestion.Answers.SingleOrDefault(x => x.Id == answer.Id);
                if(updatedAnswer != null)
                {
                    answer.Edit(updatedAnswer);
                }
                else
                {
                    answer.IsDeleted = true;
                }
            }

            Answers.AddRange(newAnswers);
        }
    }
}
