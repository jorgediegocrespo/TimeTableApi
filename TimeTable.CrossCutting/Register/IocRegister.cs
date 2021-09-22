using Microsoft.Extensions.DependencyInjection;
using TimeTable.Application.Configuration;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services;
using TimeTable.Application.Services;
using TimeTable.DataAccess;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories;

namespace TimeTable.CrossCutting.Register
{
    public static class IocRegister
    {
        public static void RegisterDbContext(IServiceCollection services, string connectionString)
        {
            DataAccess.Startup.RegisterDbContext(services, connectionString);
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static void AddRegistration(this IServiceCollection services)
        {
            RegisterRepositories(services);
            RegisterServices(services);
            RegisterOthers(services);
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<ITimeRecordRepository, TimeRecordRepository>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<ITimeRecordService, TimeRecordService>();
        }

        private static void RegisterOthers(IServiceCollection services)
        {
            services.AddTransient<IAppConfig, AppConfig>();
        }
    }
}
