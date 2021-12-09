using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Business.ConstantValues;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.DataAccess.Seed
{
    public class PeopleSeed
	{
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public PeopleSeed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task SeedAsync(TimeTableDbContext context, IConfiguration configuration)
		{
			await AddDefaultCompany(context);
		}

		private async Task AddDefaultCompany(TimeTableDbContext context)
		{
			if (context.People.Any())
				return;

			using (var transaction = await context.Database.BeginTransactionAsync())
			{
				try
				{
					IdentityUser user = new IdentityUser
					{
						UserName = "Admin",
						Email = "admin@email.com",
					};

					await userManager.CreateAsync(user, "Admin_1234");
					await userManager.AddToRoleAsync(user, RolesConsts.ADMIN);

					string userId = await userManager.GetUserIdAsync(user);
					await context.People.AddAsync(new PersonEntity() { Name = "Admin", UserId = userId });

					await context.SaveChangesAsync();

					await transaction.CommitAsync();
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}
	}
}
