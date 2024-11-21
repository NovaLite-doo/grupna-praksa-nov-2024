using FakeItEasy;
using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using Konteh.FrontOffice.Api.Features.Exams;
using System.Linq;

namespace Konteh.FrontOffice.Api.Tests
{
    public class ExamTest
    {
        private IRepository<Question> _mockQuestionRepository;
        private IRepository<Exam> _mockExamRepository;
        private CreateExam.RequestHandler _handler;
        private Random _mockRandom;

        [SetUp]
        public void Setup()
        {
            _mockQuestionRepository = A.Fake<IRepository<Question>>();
            _mockExamRepository = A.Fake<IRepository<Exam>>();

            _handler = new CreateExam.RequestHandler(_mockQuestionRepository, _mockExamRepository, _mockRandom);
            _mockRandom = A.Fake<Random>();
        }

        [Test]
        public async Task CreateExam()
        {
            //arrange
            var questions = new List<Question>
            {
                new() {
                    Id = 1,
                    Text = "What is Object-Oriented Programming (OOP)?",
                    Answers =
                    [
                        new Answer { Text = "A programming paradigm based on the concept of objects" },
                        new Answer { Text = "A type of relational database" }
                    ],
                    Category = QuestionCategory.OOP,  
                    Type = QuestionType.Radiobutton, 
                    IsDeleted = false
                },
                new() {
                    Id = 2,
                    Text = "What is Git?",
                    Answers =
                    [
                        new Answer { Text = "A version control system" },
                        new Answer { Text = "A programming language" }
                    ],
                    Category = QuestionCategory.GIT,  
                    Type = QuestionType.Radiobutton, 
                    IsDeleted = false
                },
                new() {
                    Id = 3,
                    Text = "What is SQL?",
                    Answers =
                    [
                        new Answer { Text = "A language used for managing relational databases" },
                        new Answer { Text = "A markup language for web pages" }
                    ],
                    Category = QuestionCategory.SQL, 
                    Type = QuestionType.Radiobutton,
                    IsDeleted = false
                },
                new() {
                    Id = 4,
                    Text = "What does the keyword 'class' represent in OOP?",
                    Answers =
                    [
                        new Answer { Text = "A blueprint for creating objects" },
                        new Answer { Text = "A method in programming" }
                    ],
                    Category = QuestionCategory.OOP,  
                    Type = QuestionType.Radiobutton, 
                    IsDeleted = false
                },
                new() {
                    Id = 5,
                    Text = "Which command is used to create a new Git repository?",
                    Answers =
                    [
                        new Answer { Text = "git init" },
                        new Answer { Text = "git create" }
                    ],
                    Category = QuestionCategory.GIT,
                    Type = QuestionType.Radiobutton,
                    IsDeleted = false
                },
                new() {
                    Id = 6,
                    Text = "Which SQL statement is used to extract data from a database?",
                    Answers =
                    [
                        new Answer { Text = "SELECT" },
                        new Answer { Text = "EXTRACT" }
                    ],
                    Category = QuestionCategory.SQL,
                    Type = QuestionType.Radiobutton,
                    IsDeleted = false
                },
                new() {
                    Id = 7,
                    Text = "Which SQL command is used to delete a table from a database?",
                    Answers =
                    [
                        new Answer { Text = "DROP TABLE" },
                        new Answer { Text = "DELETE TABLE" }
                    ],
                    Category = QuestionCategory.SQL,
                    Type = QuestionType.Radiobutton,
                    IsDeleted = false
                }
            };

            A.CallTo(() => _mockQuestionRepository.GetAll()).Returns(questions);
            A.CallTo(() => _mockRandom.Next()).ReturnsNextFromSequence(1, 5, 7);


            var command = new CreateExam.Command
            {
                Email = "student@example.com",
                Faculty = "Engineering",
                Major = "Computer Science",
                Name = "John",
                Surname = "Doe",
                YearOfStudy = YearOfStudy.Master
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            await Verify(result);

            //Assert.That(result.Questions.Count, Is.EqualTo(3));
            //Assert.That(result.Questions.Count(q => q.Question.Category == QuestionCategory.OOP), Is.EqualTo(1)); 
            //Assert.That(result.Questions.Count(q => q.Question.Category == QuestionCategory.GIT), Is.EqualTo(1)); 
            //Assert.That(result.Questions.Count(q => q.Question.Category == QuestionCategory.SQL), Is.EqualTo(1));

            A.CallTo(() => _mockExamRepository.SaveChanges()).MustHaveHappened();
        }
    }
}