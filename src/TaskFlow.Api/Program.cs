using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using TaskFlow.Application.DependencyInjection;
using TaskFlow.Infrastructure.AppDbContext;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using TaskFlow.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

// Database & Persistence
builder.AddSqlServerDbContext<ApplicationDbContext>("taskflow-db");

// Application Services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddFluentValidationAutoValidation();

// Swagger/OpenAPI
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskFlow API", Version = "v1" });
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
    
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    { 
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskFlow API V1"); 
    });
}

app.UseExceptionHandler();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();