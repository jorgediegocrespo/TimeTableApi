using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.DataAccess.Seed
{
    public class CompanySeed
    {
		public async Task SeedAsync(TimeTableDbContext context, IConfiguration configuration)
		{
			await AddDefaultCompany(context);
		}

        private async Task AddDefaultCompany(TimeTableDbContext context)
        {
			if (context.Companies.Any())
				return;

			await context.Companies.AddAsync(new CompanyEntity() { Name = "Default company"});
			await context.SaveChangesAsync();
		}
    }
}
