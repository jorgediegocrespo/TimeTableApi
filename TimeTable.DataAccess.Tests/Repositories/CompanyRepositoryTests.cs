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
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            await AddCompanyAsync(timeTableContext);

            CompanyRepository companyRepository = new CompanyRepository(timeTableContext);
            CompanyEntity result = await companyRepository.GetAsync();

            Assert.AreEqual("Test Company S.A.", result.Name);
        }

        [TestMethod]
        public async Task Update_OK()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            await AddCompanyAsync(timeTableContext);
            timeTableContext.ChangeTracker.Clear();

            CompanyRepository companyRepository = new CompanyRepository(timeTableContext);
            CompanyEntity toUpdate = await timeTableContext.Companies.FirstOrDefaultAsync();
            timeTableContext.ChangeTracker.Clear();

            var entityOne = await companyRepository.AttachAsync(toUpdate.Id, toUpdate.RowVersion);
            entityOne.Name = "Changed name S.L.";
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
            CompanyEntity toUpdate = await timeTableContext.Companies.FirstOrDefaultAsync();
            timeTableContext.ChangeTracker.Clear();

            var entityOne = await companyRepository.AttachAsync(toUpdate.Id, toUpdate.RowVersion);
            entityOne.Name = "Changed one S.L.";
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            var entityTwo = await companyRepository.AttachAsync(toUpdate.Id, toUpdate.RowVersion);
            entityTwo.Name = "Changed two S.L.";

            await Assert.ThrowsExceptionAsync<DbUpdateConcurrencyException>(() => timeTableContext.SaveChangesAsync());
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
