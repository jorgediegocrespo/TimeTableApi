using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimeTable.DataAccess;
using TimeTable.DataAccess.Seed;

namespace TimeTable.CrossCutting.Register
{
    public static class ContextBuilder
    {
        public static IHost BuildContext(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetService<IConfiguration>();
                var context = services.GetService<TimeTableDbContext>();
                context.Database.Migrate();


                var userManager = services.GetService<UserManager<IdentityUser>>();
                var rolesManager = services.GetService<RoleManager<IdentityRole>>();

                new CompanySeed().SeedAsync(context, configuration).Wait();
                new RolesSeed(rolesManager).SeedAsync(context, configuration).Wait();
                new PeopleSeed(userManager, rolesManager).SeedAsync(context, configuration).Wait();
            }

            return host;
        }
    }
}
