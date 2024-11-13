using Konteh.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument(o => o.SchemaSettings.SchemaNameGenerator = new CustomSwaggerSchemaNameGenerator());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseOpenApi();
app.UseSwaggerUi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
