using Argon;
using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.FrontOffice.Api.Features.Exams;
using Konteh.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;


namespace Konteh.FrontOffice.Api.Tests
{
    public class ExamControllerTest
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<AppDbContext>(options =>
                        {
                            options.UseSqlServer("Server=localhost;Database=TestDatabase;Integrated Security=True;TrustServerCertificate=True;");
                        });

                        var serviceProvider = services.BuildServiceProvider();

                        using (var scope = serviceProvider.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                            dbContext.Database.EnsureCreated();

                            var questions = LoadQuestionsFromFile("questions.json");
                            dbContext.Questions.AddRange(questions);
                            dbContext.SaveChanges();
                        }
                    });
                });
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task GenerateExam()
        {
            var command = new CreateExam.Command
            {
                Email = "lucy@gmail.com",
                Faculty = "Ftn",
                Major = "RA",
                Name = "Lucy",
                Surname = "Bing",
                YearOfStudy = YearOfStudy.Master
            };

            var result = await _client.PostAsJsonAsync("/exams", command);

            Assert.That(result.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var jsoncontent = await result.Content.ReadAsStringAsync();
            var examId = JsonConvert.DeserializeObject<int>(jsoncontent);

            var examResponse = await _client.GetAsync($"/exams/{examId}");
            var examJson = await examResponse.Content.ReadAsStringAsync();
            var retrievedExam = JsonConvert.DeserializeObject<Exam>(examJson);

            Assert.That(retrievedExam, Is.Not.Null);

            await Verify(retrievedExam).IgnoreMembers("Id");
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        private List<Question> LoadQuestionsFromFile(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Question>>(json);
        }
    }
}
