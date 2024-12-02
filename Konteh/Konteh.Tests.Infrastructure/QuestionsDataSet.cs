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
                },
                new Question{
                    Text = "What is the purpose of the 'git pull' command?",
                    Category = Domain.Enumeration.QuestionCategory.SQL,
                    Type = Domain.Enumeration.QuestionType.Radiobutton,
                    Answers = [
                        new(){Text = "To fetch changes from a remote repository and merge them"},
                        new(){Text ="To upload changes to a remote repository" }
                        ]
                },
                new Question
                {
                    Text = "In Git, what is the difference between 'git merge' and 'git rebase'?",
                    Category = Domain.Enumeration.QuestionCategory.SQL,
                    Type = Domain.Enumeration.QuestionType.Radiobutton,
                    Answers = [
                        new(){Text = "Merge combines the histories of two branches, rebase re-applies commits on top of another branch"},
                        new(){Text ="Merge re-applies commits on top of another branch, rebase combines histories" }
                        ]
                },
                new Question
                {
                    Text = "What is an OOP concept that allows one object to take on multiple forms?",
                    Category = Domain.Enumeration.QuestionCategory.SQL,
                    Type = Domain.Enumeration.QuestionType.Radiobutton,
                    Answers = [
                        new(){Text = "Polymorphism"},
                        new(){Text ="Inheritance" }
                        ]
                },
                new Question
                {
                    Text = "Which of the following is NOT a characteristic of Object-Oriented Programming (OOP)?",
                    Category = Domain.Enumeration.QuestionCategory.OOP,
                    Type = Domain.Enumeration.QuestionType.Radiobutton,
                    Answers = [
                        new(){Text = "Encapsulation"},
                        new(){Text ="Polymorphisme" },
                        new() { Text = "Functionality" }
                        ]
                },
                new Question
                {
                    Text = "What is the purpose of the 'super' keyword in OOP?",
                    Category = Domain.Enumeration.QuestionCategory.OOP,
                    Type = Domain.Enumeration.QuestionType.Radiobutton,
                    Answers = [
                        new(){Text = "To call a method or constructor from the parent class"},
                        new(){Text ="To define a new object" }
                        ]
                },
                new Question
                {
                    Text = "Which of the following describes inheritance in OOP?",
                    Category = Domain.Enumeration.QuestionCategory.OOP,
                    Type = Domain.Enumeration.QuestionType.Radiobutton,
                    Answers = [
                        new(){Text = "A version control system"},
                        new(){Text ="A programming language" }
                        ]
                },
                new Question
                {
                    Text = "Which of the following describes inheritance in OOP?",
                    Category = Domain.Enumeration.QuestionCategory.OOP,
                    Type = Domain.Enumeration.QuestionType.Radiobutton,
                    Answers = [
                        new(){Text = "A class can inherit properties and methods from another class"},
                        new(){Text ="A class can inherit only methods from another class" }
                        ]
                },
                new Question
                {
                    Text = "In OOP, what is the term for the blueprint used to create objects?",
                    Category = Domain.Enumeration.QuestionCategory.OOP,
                    Type = Domain.Enumeration.QuestionType.Radiobutton,
                    Answers = [
                        new(){Text = "Class"},
                        new(){Text ="Object" }
                        ]
                },
                new Question
                {
                    Text = "Which OOP principle promotes data hiding and protection?",
                    Category = Domain.Enumeration.QuestionCategory.OOP,
                    Type = Domain.Enumeration.QuestionType.Radiobutton,
                    Answers = [
                        new(){Text = "Encapsulation"},
                        new(){Text ="Polymorphism" }
                        ]
                },
        ];
    }
}
