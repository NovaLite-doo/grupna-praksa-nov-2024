using Konteh.Domain;
using Konteh.Infrastructure;
using Konteh.Infrastructure.Options;
using Konteh.Infrastructure.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        builder.Services.AddControllers();
        builder.Services.AddOpenApiDocument(o => o.SchemaSettings.SchemaNameGenerator = new CustomSwaggerSchemaNameGenerator());

        builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IRepository<Question>, QuestionRepository>();
        builder.Services.AddScoped<IRepository<Exam>, ExamRepository>();
        builder.Services.AddScoped<IRepository<Candidate>, CandidateRepository>();
        builder.Services.AddScoped<IRandom, KontehRandom>();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        builder.Services.Configure<RabbitMqOptions>(
            builder.Configuration.GetSection(RabbitMqOptions.RabbitMq));

        builder.Services.AddMassTransit(cfg =>
        {
            builder.Services.Configure<RabbitMqOptions>(
                builder.Configuration.GetSection(RabbitMqOptions.RabbitMq));
            cfg.SetKebabCaseEndpointNameFormatter();

            cfg.UsingRabbitMq((context, configurator) =>
            {
                var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                configurator.Host(rabbitMqOptions.Host, "/", h =>
                {
                    h.Username(rabbitMqOptions.Username);
                    h.Password(rabbitMqOptions.Password);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseOpenApi();
        app.UseSwaggerUi();

        app.UseCors(MyAllowSpecificOrigins);

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}