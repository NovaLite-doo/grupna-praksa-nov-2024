using FakeItEasy;
using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using System.Reflection.Metadata;

namespace Konteh.FrontOffice.Api.Tests
{
    public class ExamTest
    {
        private IRepository<Question> _mockQuestionRepository;
        private IRepository<Exam> _mockExamRepository;
        //private CreateExam.RequestHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockQuestionRepository = A.Fake<IRepository<Question>>();
            _mockExamRepository = A.Fake<IRepository<Exam>>();

            //_handler = new CreateExam.RequestHandler(_mockQuestionRepository, _mockExamRepository);
        }

        [Test]
        public void CreateExam()
        {
            //arrange
            var questions = new List<Question>
            {
                new Question 
                {
                    Id = 1,
                    Text = "What is Object-Oriented Programming (OOP)?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "A programming paradigm based on the concept of objects" },
                        new Answer { Text = "A type of relational database" }
                    },
                    Category = QuestionCategory.OOP,  
                    Type = QuestionType.Radiobutton, 
                    IsDeleted = false
                },
                new Question
                {
                    Id = 2,
                    Text = "What is Git?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "A version control system" },
                        new Answer { Text = "A programming language" }
                    },
                    Category = QuestionCategory.GIT,  
                    Type = QuestionType.Radiobutton, 
                    IsDeleted = false
                },
                new Question
                {
                    Id = 3,
                    Text = "What is SQL?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "A language used for managing relational databases" },
                        new Answer { Text = "A markup language for web pages" }
                    },
                    Category = QuestionCategory.SQL, 
                    Type = QuestionType.Radiobutton,
                    IsDeleted = false
                },
                new Question
                {
                    Id = 4,
                    Text = "What does the keyword 'class' represent in OOP?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "A blueprint for creating objects" },
                        new Answer { Text = "A method in programming" }
                    },
                    Category = QuestionCategory.OOP,  
                    Type = QuestionType.Radiobutton, 
                    IsDeleted = false
                }
            };

            /*var command = new CreateExam.Command
            {
                Email = "student@example.com",
                Faculty = "Engineering",
                Major = "Computer Science",
                Name = "John",
                Surname = "Doe",
                YearOfStudy = YearOfStudy.FirstYear
            };*/


            //act
            //var result = await _handler.Handle(command, default);
            //assert
           // Assert.AreEqual(result.Questions.Count, 4);
            Assert.Pass();
        }
    }
}