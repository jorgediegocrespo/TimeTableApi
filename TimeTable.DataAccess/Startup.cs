using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimeTable.DataAccess.Contracts;

namespace TimeTable.DataAccess
{
    public static class Startup
    {
        public static void RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddTransient<ITimeTableDbContext, TimeTableDbContext>();
            services.AddDbContext<TimeTableDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
