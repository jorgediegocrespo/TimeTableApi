using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Business.ConstantValues;

namespace TimeTable.DataAccess.Seed
{
    public class RolesSeed
    {
		private readonly RoleManager<IdentityRole> roleManager;

		public RolesSeed(RoleManager<IdentityRole> roleManager)
		{
			this.roleManager = roleManager;
		}

		public async Task SeedAsync(TimeTableDbContext context, IConfiguration configuration)
		{
			await AddDefaultCompany(context);
		}

		private async Task AddDefaultCompany(TimeTableDbContext context)
		{
			if (context.Roles.Any())
				return;

			await roleManager.CreateAsync(new IdentityRole(RolesConsts.ADMIN));
			await roleManager.CreateAsync(new IdentityRole(RolesConsts.EMPLOYEE));
		}
	}
}
