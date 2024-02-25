using SkillMasteryAPI.Application;
using SkillMasteryAPI.Infrastructure;
using SkillMasteryAPI.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ***********  DEPENDENCY INJECTION ************
builder.Services.AddApplicationServices();
builder.Services.AddInfraestructureRepositories();

// ***********  HEALTHCHECK ************
builder.Services.AddHealthChecks();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
