using FluentValidation;
using Konteh.BackOffice.Api.Featuers.Exams.ExamNotifications;
using Konteh.Domain;
using Konteh.Infrastructure;
using Konteh.Infrastructure.ExceptionHandling;
using Konteh.Infrastructure.Options;
using Konteh.Infrastructure.Repository;
using Konteh.Infrastructure.Validation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
builder.Services.AddScoped<IRepository<Exam>, ExamRepository>();
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


builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection(RabbitMqOptions.RabbitMq));


builder.Services.AddMassTransit(cfg =>
{
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

    cfg.AddConsumer<ExamNotificationHandler>();

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

app.MapHub<ExamNotificationHub>(ExamNotificationHub.Route);

app.Run();