using Konteh.Domain;

namespace Konteh.Tests.Infrastructure
{
    public static class QuestionsDataSet
    {
        public static IEnumerable<Question> GetQuestions() =>
            [
            new Question{
                Text = "What is Object-Oriented Programming (OOP)?",
                Category = Domain.Enumeration.QuestionCategory.OOP,
                Type = Domain.Enumeration.QuestionType.Radiobutton,
                Answers = [
                    new(){Text = "A programming paradigm based on the concept of objects"},
                    new(){Text = "A type of relational database"}
                    ]
            },
            new Question{
                Text = "What is Git?",
                Category = Domain.Enumeration.QuestionCategory.GIT,
                Type = Domain.Enumeration.QuestionType.Radiobutton,
                Answers = [
                    new(){Text = "A version control system"},
                    new(){Text ="A programming language" }
                    ]
            }
            ];
    }
}
