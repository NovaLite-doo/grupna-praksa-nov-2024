using Konteh.Infrastructure;
using Konteh.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using System.Reflection;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApiDocument(o => o.SchemaSettings.SchemaNameGenerator = new CustomSwaggerSchemaNameGenerator());


        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                .AddMicrosoftIdentityWebApi(builder.Configuration, "AzureAd");

        builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


        builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

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


        var app = builder.Build();

        var environment = app.Environment.EnvironmentName;
        Console.WriteLine($"Current Environment: {environment}");

        // Configure the HTTP request pipeline.

        app.UseOpenApi();
        app.UseSwaggerUi();
        app.UseCors(MyAllowSpecificOrigins);

        app.UseHttpsRedirection();
        if (environment != "Testing")
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }


        app.MapControllers();

        app.Run();
    }
}