using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

        public static void RegisterIdentity(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<TimeTableDbContext>()
                    .AddDefaultTokenProviders();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<ITimeRecordRepository, TimeRecordRepository>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddSingleton<IFileStorage, BlobAzureStorage>();
            services.AddTransient<IUserService, UserService>();
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
