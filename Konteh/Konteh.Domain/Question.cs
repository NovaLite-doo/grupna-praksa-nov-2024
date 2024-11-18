using Konteh.Domain.Enumeration;
using System.Net;

namespace Konteh.Domain
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<Answer> Answers { get; } = [];
        public QuestionCategory Category { get; set; }
        public QuestionType Type { get; set; }

        public void Validate()
        {
            if(string.IsNullOrEmpty(Text))
            {
                throw new ArgumentException($"{nameof(Question)} {nameof(Text)} is mandatory");
            }

            foreach (var answer in Answers)
            {
                answer.Validate();
            }
        }

        public void AddAnswers(IEnumerable<Answer> answers)
        {
            Answers.AddRange(answers);
        }

        public void Edit(Question question)
        {
            question.Validate();

            Text = question.Text;
            Category = question.Category;
            Type = question.Type;

            foreach (var answer in question.Answers)
            {
                var originalAnswer = Answers.Find(x => x.Id == answer.Id);
                if (originalAnswer != null)
                {
                    originalAnswer.Edit(answer);
                }
                else
                {
                    Answers.Add(answer);
                }
            }
        }
    }
}
