using Konteh.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Respawn;

namespace Konteh.Tests.Infrastructure
{
    public abstract class BaseIntegrationFixture<TProgram> where TProgram : class
    {
        protected WebApplicationFactory<TProgram> _factory = null!;
        protected HttpClient _client = null!;
        private string _connectionString = "Server=localhost;Database=KontehTest;Integrated Security=True;TrustServerCertificate=True;";
        private Respawner _respawner = null!;
        private Action<IServiceCollection> _action;

        protected BaseIntegrationFixture(Action<IServiceCollection> action)
        {
            _action = action;
        }

        [SetUp]
        public async Task SetUp()
        {
            _factory = new WebApplicationFactory<TProgram>()
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

                        _action(services);

                    });
                });

            _client = _factory.CreateClient();
            var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Database.MigrateAsync();

            _respawner = await Respawner.CreateAsync(_connectionString, new RespawnerOptions
            {
                TablesToIgnore = ["__EFMigrationsHistory"]
            });

            await _respawner.ResetAsync(_connectionString);
        }

        protected async Task AddQuestions()
        {
            var dbContext = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.AddRange(QuestionsDataSet.GetQuestions());
            await dbContext.SaveChangesAsync();
        }
    }
}
