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


        public void Edit(Question question, IEnumerable<Answer> newAnswers)
        {
            Text = question.Text;
            Category = question.Category;
            Type = question.Type;

            var answersToDelete = Answers.Where(x => !question.Answers.Select(a => a.Id).Contains(x.Id)).ToList();
            foreach (var answerToDelete in answersToDelete)
            {
                //TODO logical deletion
                Answers.Remove(answerToDelete);
            }

            foreach (var answer in Answers)
            {
                var updatedAnswer = question.Answers.Single(x => x.Id == answer.Id);
                if(updatedAnswer != null)
                {
                    answer.Edit(updatedAnswer);
                }
            }

            Answers.AddRange(newAnswers);
        }
    }
}
