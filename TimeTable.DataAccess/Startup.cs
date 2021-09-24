using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TimeTable.DataAccess
{
    public static class Startup
    {
        public static void RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<TimeTableDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
        }
    }
}
