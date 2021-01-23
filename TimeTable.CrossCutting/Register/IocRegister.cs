using Microsoft.Extensions.DependencyInjection;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories;

namespace TimeTable.CrossCutting.Register
{
    public static class IocRegister
    {
        public static void RegisterDbContext(IServiceCollection services, string connectionString)
        {
            DataAccess.Startup.RegisterDbContext(services, connectionString);
        }

        public static void AddRegistration(this IServiceCollection services)
        {
            RegisterRepositories(services);
            RegisterServices(services);
            RegisterOthers(services);
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IBankDayRepository, BankDayRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IHolidayRepository, HolidayRepository>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IVacationDayRepository, VacationDayRepository>();
        }

        private static void RegisterServices(IServiceCollection services)
        { }

        private static void RegisterOthers(IServiceCollection services)
        { }
    }
}
