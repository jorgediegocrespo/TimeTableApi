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
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            await AddCompanyAsync(timeTableContext);

            CompanyRepository companyRepository = new CompanyRepository(timeTableContext);
            CompanyEntity result = await companyRepository.GetAsync();

            Assert.AreEqual("Test Company S.A.", result.Name);
        }

        [TestMethod]
        public async Task Update_OK()
        {
            //TODO Use local DB
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            await AddCompanyAsync(timeTableContext);

            CompanyRepository companyRepository = new CompanyRepository(timeTableContext);
            CompanyEntity company = await timeTableContext.Companies.FirstOrDefaultAsync();
            company.Name = "Changed name S.L.";
            await companyRepository.UpdateAsync(company);
            await timeTableContext.SaveChangesAsync();

            CompanyEntity result = await companyRepository.GetAsync();
            Assert.AreEqual("Changed name S.L.", result.Name);
        }

        private async Task AddCompanyAsync(TimeTableDbContext timeTableContext)
        {
            await timeTableContext.Companies.AddAsync(new CompanyEntity
            {
                Name = "Test Company S.A."
            });
            await timeTableContext.SaveChangesAsync();
        }
    }
}
