
using SkillMasteryAPI.Infrastructure;
using SkillMasteryAPI.Infrastructure.Data;
using SkillMasteryAPI.Application;
using SkillMasteryAPI.Presentation.Middlewares;


using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ResponseCompression;

namespace SkillMasteryAPI.Presentation;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder = ConfigureServicesAndMiddlewares(builder);


        var app = builder.Build();
        app.UseResponseCompression();
        // Configure the HTTP request pipeline.
   
        
            app.UseSwagger();
            app.UseSwaggerUI();
        
        app.UseExceptionHandler();
        app.UseCors(c =>
        {
            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
            if (allowedOrigins is not null)
                c.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("/health");
        app.Run();
    }

    public static WebApplicationBuilder ConfigureServicesAndMiddlewares(WebApplicationBuilder builder)
    {
        // ***********  API CONTROLLERS AND RESPONSES ************

        builder.Services.AddControllers(configure =>
        {
            configure.ReturnHttpNotAcceptable = true;
            configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
            configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
            configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
        }).AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });

        // ***********  FLUENT VALIDATION ************
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();



        // ***********  API SWAGGER GEN ************
        builder.Services.AddSwaggerGen();

            builder.Services.AddRouting(options => options.LowercaseUrls = true);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        
        // ***********  GZIP COMPRESSION ************
        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
        });


        // ***********  EXCEPTIONS ************
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        // ***********  DEPENDENCY INJECTION ************
        builder.Services.AddApplicationServices();
        builder.Services.AddInfraestructureRepositories();

        // ***********  HEALTHCHECK ************
        builder.Services.AddHealthChecks();

        // ***********  DBCONTEXT ************
        builder.Services.AddDbContext<SkillMasteryContext>(options =>
           options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        return builder;
    }
}
