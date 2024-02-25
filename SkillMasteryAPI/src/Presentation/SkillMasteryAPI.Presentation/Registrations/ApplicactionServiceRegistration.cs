using System.Reflection;
using Mapster;
using MapsterMapper;
using SkillMasteryAPI.Application.Services;
using SkillMasteryAPI.Application.Services.Interfaces;


namespace SkillMasteryAPI.Application;
public static class ApplicactionServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Mapster configuration, this scans all custom configs
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services
            .AddScoped<ISkillService, SkillService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IGoalService, GoalService>()
            .AddScoped<IUserSkillService, UserSkillService>()
            .AddSingleton(config)
            .AddScoped<IMapper, ServiceMapper>();
        return services;

       
    }
}
