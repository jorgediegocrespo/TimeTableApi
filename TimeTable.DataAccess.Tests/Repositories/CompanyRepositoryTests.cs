using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Repositories;
using TimeTable.DataAccess.Tests.Repositories.Base;

namespace TimeTable.DataAccess.Tests.Repositories
{
    [TestClass]
    public class CompanyRepositoryTests : BaseRepositoryTests
    {
        [TestMethod]
        public async Task Get_OK()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            await AddCompanyAsync(timeTableContext);

            CompanyRepository companyRepository = new CompanyRepository(timeTableContext);
            CompanyEntity result = await companyRepository.GetAsync();

            Assert.AreEqual("Test Company S.A.", result.Name);
        }

        [TestMethod]
        public async Task Update_OK()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            await AddCompanyAsync(timeTableContext);
            timeTableContext.ChangeTracker.Clear();

            CompanyRepository companyRepository = new CompanyRepository(timeTableContext);
            CompanyEntity company = await timeTableContext.Companies.FirstOrDefaultAsync();
            company.Name = "Changed name S.L.";
            await companyRepository.UpdateAsync(company);
            await timeTableContext.SaveChangesAsync();

            timeTableContext.ChangeTracker.Clear();
            CompanyEntity result = await companyRepository.GetAsync();
            Assert.AreEqual("Changed name S.L.", result.Name);
        }

        [TestMethod]
        public async Task Update_ConcurrencyError()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            await AddCompanyAsync(timeTableContext);
            timeTableContext.ChangeTracker.Clear();

            CompanyRepository companyRepository = new CompanyRepository(timeTableContext);
            CompanyEntity companyOne = await timeTableContext.Companies.FirstOrDefaultAsync();
            companyOne.Name = "Changed one S.L.";

            await companyRepository.UpdateAsync(companyOne);
            await timeTableContext.SaveChangesAsync();

            CompanyEntity companyTwo = await timeTableContext.Companies.FirstOrDefaultAsync();
            companyTwo.Name = "Changed two S.L.";

            await companyRepository.UpdateAsync(companyTwo);
            await timeTableContext.SaveChangesAsync();

            //await Assert.ThrowsExceptionAsync<DbUpdateException>(result);
            await timeTableContext.DisposeAsync();
        }

        private async Task AddCompanyAsync(TimeTableDbContext timeTableContext)
        {
            await timeTableContext.Companies.AddAsync(new CompanyEntity
            {
                Name = "Test Company S.A.",
            });
            await timeTableContext.SaveChangesAsync();
        }
    }
}
