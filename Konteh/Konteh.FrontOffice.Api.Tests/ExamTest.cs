using FakeItEasy;
using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.FrontOffice.Api.Features.Exams;
using Konteh.Infrastructure.Repository;
using Konteh.Tests.Infrastructure;

namespace Konteh.FrontOffice.Api.Tests
{
    public class ExamTest
    {
        private IRepository<Question> _mockQuestionRepository;
        private IRepository<Exam> _mockExamRepository;
        private IRepository<Candidate> _mockCandidateRepository;
        private CreateExam.RequestHandler _handler;
        private Random _mockRandom;

        [SetUp]
        public void Setup()
        {
            _mockQuestionRepository = A.Fake<IRepository<Question>>();
            _mockExamRepository = A.Fake<IRepository<Exam>>();
            _mockCandidateRepository = A.Fake<IRepository<Candidate>>();
            _mockRandom = A.Fake<Random>();

            _handler = new CreateExam.RequestHandler(_mockQuestionRepository, _mockExamRepository, _mockRandom, _mockCandidateRepository);
        }

        [Test]
        public async Task CreateExam()
        {
            var questions = QuestionsDataSet.GetQuestions().ToList();

            A.CallTo(() => _mockQuestionRepository.GetAll()).Returns(questions);

            A.CallTo(() => _mockRandom.Next(0, A<int>.Ignored)).ReturnsNextFromSequence(0, 1, 2);

            A.CallTo(() => _mockCandidateRepository.Create(A<Candidate>.Ignored)).Invokes((Candidate candidate) => { });

            var command = new CreateExam.Command
            {
                Email = "student@example.com",
                Faculty = "Engineering",
                Major = "Computer Science",
                Name = "John",
                Surname = "Doe",
                YearOfStudy = YearOfStudy.Master
            };

            var examId = await _handler.Handle(command, CancellationToken.None);

            var exam = await _mockExamRepository.Get(examId);

            await Verify(exam.Questions);

            A.CallTo(() => _mockExamRepository.SaveChanges()).MustHaveHappened();
        }
    }
}