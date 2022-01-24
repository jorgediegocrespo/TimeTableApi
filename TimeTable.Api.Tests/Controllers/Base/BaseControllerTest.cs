using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TimeTable.Business.ConstantValues;
using TimeTable.DataAccess;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Api.Tests.Controllers.Base
{
    public class BaseControllerTest
    {
        private readonly string personUrl = "/api/person";

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

                    services.AddSingleton<IAuthorizationHandler, AllowAnonimousHandler>();
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
                Id = "724cd46f-814a-449c-b80b-f360228437f8",
                UserName = "Admin",
                Email = "admin@email.com",
            };

            await userManager.CreateAsync(user, "Admin_1234");
            await userManager.AddToRoleAsync(user, RolesConsts.ADMIN);

            string userId = await userManager.GetUserIdAsync(user);
            await context.People.AddAsync(new PersonEntity() { Id = 1, Name = "Admin", UserId = userId, IsDefault = true });

            await context.SaveChangesAsync();
        }

        private async Task AddEmployeePerson(UserManager<IdentityUser> userManager, TimeTableDbContext context)
        {
            if (context.People.Any())
                return;

            IdentityUser user = new IdentityUser
            {
                Id = "0d3e5510-1ec3-41bb-9ed8-66da900a108e",
                UserName = "Employee",
                Email = "employee@email.com",
            };

            await userManager.CreateAsync(user, "Employee_1234");
            await userManager.AddToRoleAsync(user, RolesConsts.EMPLOYEE);

            string userId = await userManager.GetUserIdAsync(user);
            await context.People.AddAsync(new PersonEntity() { Id = 2, Name = "Employee", UserId = userId, IsDefault = true });

            await context.SaveChangesAsync();
        }
    }

    public class FakeUserFilter : IAsyncActionFilter
    {
        private readonly string role;

        public FakeUserFilter(string role)
        {
            this.role = role;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            switch (role)
            {
                case RolesConsts.ADMIN:
                    context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, "724cd46f-814a-449c-b80b-f360228437f8"),
                        new Claim(ClaimTypes.Name, "Admin"),
                        new Claim(ClaimTypes.Email, "admin@email.com"),
                    }, "fakeAdminTest"));
                    break;
                case RolesConsts.EMPLOYEE:
                    context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, "0d3e5510-1ec3-41bb-9ed8-66da900a108e"),
                        new Claim(ClaimTypes.Name, "Employee"),
                        new Claim(ClaimTypes.Email, "employee@email.com"),
                    }, "fakeEmployeeTest"));
                    break;
            }

            await next();
        }
    }

    public class AllowAnonimousHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (var requierements in context.PendingRequirements.ToList())
                context.Succeed(requierements);

            return Task.CompletedTask;
        }
    }
}
