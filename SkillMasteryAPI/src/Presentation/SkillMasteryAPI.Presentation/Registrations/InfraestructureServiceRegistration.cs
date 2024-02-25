using SkillMasteryAPI.Infrastructure.Repositories;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;

namespace SkillMasteryAPI.Infrastructure;

public static class InfraestructureServiceRegistration
{
        public static IServiceCollection AddInfraestructureRepositories(this IServiceCollection services)
        {
                services.AddScoped<IProgramRepository, ProgramRepository>();
                services.AddScoped<ICourseRepository, CourseRepository>();
                services.AddScoped<IMeetingRepository, MeetingRepository>();
                services.AddScoped<IClassroomRepository, ClassroomRepository>();
                services.AddScoped<IScheduleRepository, ScheduleRepository>();
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<ICountryRepository, CountryRepository>();
                return services;
        }
}
