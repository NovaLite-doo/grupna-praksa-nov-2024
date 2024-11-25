using FluentValidation;
using Konteh.BackOffice.Api.Featuers.Exams;
using Konteh.Infrastructure;
using Konteh.Infrastructure.ExceptionHandling;
using Konteh.Infrastructure.Repository;
using Konteh.Infrastructure.Validation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument(o => o.SchemaSettings.SchemaNameGenerator = new CustomSwaggerSchemaNameGenerator());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApi(builder.Configuration, "AzureAd");

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSignalR();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

builder.Services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();

    var rabbitMqHost = builder.Configuration["RabbitMq:Host"];
    var rabbitMqUsername = builder.Configuration["RabbitMq:Username"];
    var rabbitMqPassword = builder.Configuration["RabbitMq:Password"];
    cfg.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(rabbitMqHost, "/", h =>
        {
            h.Username(rabbitMqUsername!);
            h.Password(rabbitMqPassword!);
        });

        configurator.ConfigureEndpoints(context);
    });

    cfg.AddConsumer<ExamNotifications.NotificationConsumer>();

});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseOpenApi();
app.UseSwaggerUi();
app.UseCors(MyAllowSpecificOrigins);

app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ExamNotifications.NotificationHub>("exam-notification-hub");

app.Run();