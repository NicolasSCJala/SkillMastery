using SkillMasteryAPI.Infrastructure.Repositories;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;

namespace SkillMasteryAPI.Infrastructure;

public static class InfraestructureServiceRegistration
{
        public static IServiceCollection AddInfraestructureRepositories(this IServiceCollection services)
        {
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<ISkillRepository, SkillRepository>();
                services.AddScoped<IUserSkillRepository, UserSkillRepository>();
                services.AddScoped<IGoalRepository, GoalRepository>();
                return services;
        }
}
