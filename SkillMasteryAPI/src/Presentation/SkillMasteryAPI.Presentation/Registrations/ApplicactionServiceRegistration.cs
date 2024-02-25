using System.Reflection;
using Mapster;
using MapsterMapper;
using FluentValidation;
/* using to use after creation of services
using SkillMasteryAPI.Application.Services;
using SkillMasteryAPI.Application.Services.Interfaces;
using SkillMasteryAPI.Application.Validators;
*/

namespace SkillMasteryAPI.Application;
public static class ApplicactionServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Mapster configuration, this scans all custom configs
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        /*
        services
            .AddScoped<ISkillService, SkillService>()
      
            

        services
            .AddValidatorsFromAssemblyContaining<CreateSkillDTOValidator>()
         */
        return services;

       
    }
}
