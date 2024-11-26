using Konteh.BackOffice.Api.Featuers.Questions;
using Konteh.Domain;
using Konteh.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Respawn;

namespace Konteh.BackOffice.Api.Tests
{
    public class QuestionControllerTest
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private string _connectionString = "Server=localhost;Database=TestDatabase;Integrated Security=True;TrustServerCertificate=True;";
        private Respawner _respawner;

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
                            options.UseSqlServer(_connectionString);
                        });

                        var serviceProvider = services.BuildServiceProvider();

                        using (var scope = serviceProvider.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                            var connection = dbContext.Database.GetDbConnection();

                            if (connection.State != System.Data.ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            var respawnOptions = new RespawnerOptions
                            {
                                DbAdapter = DbAdapter.SqlServer
                            };

                            _respawner = Respawner.CreateAsync(dbContext.Database.GetDbConnection(), respawnOptions).Result;
                            ResetDatabaseAsync(dbContext).Wait();

                            var questions = LoadQuestionsFromFile("questions.json");
                            dbContext.Questions.AddRange(questions);
                            dbContext.SaveChanges();
                        }
                    });
                });
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task Test_SearchQuestions()
        {
            var queryParameters = new Dictionary<string, string>
            {
                { "searchText", "Git" },
                { "pageNumber", "1" },
                { "pageSize", "10" },
                { "category", "GIT" }
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString("/questions/search", queryParameters));

            Assert.IsTrue(response.IsSuccessStatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SearchQuestions.PagedResponse>(content);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Questions);
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

        private async Task ResetDatabaseAsync(AppDbContext dbContext)
        {
            await _respawner.ResetAsync(dbContext.Database.GetDbConnection());
        }

    }
}