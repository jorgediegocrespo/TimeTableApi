using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Business.ConstantValues;
using TimeTable.DataAccess;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Api.Tests.Controllers.Base
{
    public class BaseControllerTest
    {
        protected TimeTableDbContext BuildContext(string dbContextName)
        {
            DbContextOptions<TimeTableDbContext> options = new DbContextOptionsBuilder<TimeTableDbContext>().UseInMemoryDatabase(dbContextName).Options;
            return new TimeTableDbContext(options);
        }

        protected WebApplicationFactory<Startup> BuildWebApplicationFactory(string dbContextName, string userRole)
        {
            WebApplicationFactory<Startup> factory = new WebApplicationFactory<Startup>();
            factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    ServiceDescriptor descriptorDbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TimeTableDbContext>));
                    if (descriptorDbContext != null)
                        services.Remove(descriptorDbContext);

                    services.AddDbContext<TimeTableDbContext>(options => options.UseInMemoryDatabase(dbContextName));

                    services.AddSingleton<IAuthorizationHandler>(x => new ManageRoleAccessHandler(userRole));
                    services.AddControllers(opt => opt.Filters.Add(new FakeUserFilter(userRole)));
                });
            });

            SeedDatabaseContext(factory);

            return factory;
        }

        private void SeedDatabaseContext(WebApplicationFactory<Startup> webApplicationFactory)
        {
            using (var scope = webApplicationFactory.Services.CreateScope())
            {
                TimeTableDbContext context = scope.ServiceProvider.GetRequiredService<TimeTableDbContext>();
                IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                IServiceProvider serviceProvider = scope.ServiceProvider;

                InitializeDbForTests(context, configuration, serviceProvider);
            }
        }

        private void InitializeDbForTests(TimeTableDbContext context, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            UserManager<IdentityUser> userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole> rolesManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            AddDefaultCompany(context).Wait();
            AddDefaultRoles(rolesManager, context).Wait();
            AddAdminPerson(userManager, context).Wait();
            AddEmployeePerson(userManager, context).Wait();
        }

        private async Task AddDefaultCompany(TimeTableDbContext context)
        {
            if (context.Companies.Any())
                return;

            await context.Companies.AddAsync(new CompanyEntity() { Name = "Default company" });
            await context.SaveChangesAsync();
        }

        private async Task AddDefaultRoles(RoleManager<IdentityRole> roleManager, TimeTableDbContext context)
        {
            if (context.Roles.Any())
                return;

            await roleManager.CreateAsync(new IdentityRole(RolesConsts.ADMIN));
            await roleManager.CreateAsync(new IdentityRole(RolesConsts.EMPLOYEE));
        }

        private async Task AddAdminPerson(UserManager<IdentityUser> userManager, TimeTableDbContext context)
        {
            if (context.People.Any())
                return;

            IdentityUser user = new IdentityUser
            {
                Id = Constants.AdminNameIdentifier,
                UserName = Constants.AdminName,
                Email = Constants.AdminEmail,
            };

            await userManager.CreateAsync(user, Constants.AdminPassword);
            await userManager.AddToRoleAsync(user, RolesConsts.ADMIN);

            string userId = await userManager.GetUserIdAsync(user);
            await context.People.AddAsync(new PersonEntity() { Id = 1, Name = Constants.AdminName, UserId = userId, IsDefault = true });

            await context.SaveChangesAsync();
        }

        private async Task AddEmployeePerson(UserManager<IdentityUser> userManager, TimeTableDbContext context)
        {
            if (context.People.Any())
                return;

            IdentityUser user = new IdentityUser
            {
                Id = Constants.EmployeeNameIdentifier,
                UserName = Constants.EmployeeName,
                Email = Constants.EmployeeEmail,
            };

            await userManager.CreateAsync(user, Constants.EmployeePassword);
            await userManager.AddToRoleAsync(user, RolesConsts.EMPLOYEE);

            string userId = await userManager.GetUserIdAsync(user);
            await context.People.AddAsync(new PersonEntity() { Id = 2, Name = Constants.EmployeePassword, UserId = userId, IsDefault = true });

            await context.SaveChangesAsync();
        }
    }
}
