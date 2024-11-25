using Konteh.Domain;
using Konteh.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Konteh.FrontOffice.Api.Tests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
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

        }

        private List<Question> LoadQuestionsFromFile(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Fajl sa pitanjima nije pronađen: {filePath}");
            }

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Question>>(json);
        }




    }
}
