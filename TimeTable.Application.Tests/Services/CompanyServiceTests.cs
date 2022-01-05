using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using TimeTable.Application.Constants;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Exceptions;
using TimeTable.Application.Services;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;

namespace TimeTable.Application.Tests.Services
{
    [TestClass]
    public class CompanyServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<ICompanyRepository> companyRepositoryMock;
        private readonly Mock<IAppConfig> appConfigMock;

        public CompanyServiceTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            companyRepositoryMock = new Mock<ICompanyRepository>();
            appConfigMock = new Mock<IAppConfig>();
        }

        [TestMethod]
        public async Task GetCompany_Ok()
        {
            CompanyEntity givenCompany = GivenCompany();
            companyRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(givenCompany);
            
            CompanyService companyService = new CompanyService(unitOfWorkMock.Object, companyRepositoryMock.Object, appConfigMock.Object);
            Company company = await companyService.GetAsync();

            Assert.AreEqual(givenCompany.Id, company.Id);
            Assert.AreEqual(givenCompany.Name, company.Name);
        }

        [TestMethod]
        public async Task UpdateCompany_IdError()
        {
            CompanyEntity givenCompany = GivenCompany();
            companyRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(givenCompany);
            companyRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CompanyEntity>())).Returns(Task.CompletedTask);

            CompanyService companyService = new CompanyService(unitOfWorkMock.Object, companyRepositoryMock.Object, appConfigMock.Object);
            Company company = new Company()
            {
                Id = 2,
                Name = "New company"
            };

            await Assert.ThrowsExceptionAsync<NotValidOperationException>(() => companyService.UpdateAsync(company), ErrorCodes.ITEM_NOT_EXISTS);
        }

        [TestMethod]
        public void UpdateCompany_Ok()
        {
            CompanyEntity givenCompany = GivenCompany();
            companyRepositoryMock.Setup(x => x.GetAsync()).ReturnsAsync(givenCompany);
            companyRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CompanyEntity>())).Returns(Task.CompletedTask);

            CompanyService companyService = new CompanyService(unitOfWorkMock.Object, companyRepositoryMock.Object, appConfigMock.Object);
            Company company = new Company()
            {
                Id = 1,
                Name = "New company"
            };
            Task result = companyService.UpdateAsync(company);

            Assert.AreEqual(Task.CompletedTask, result);
        }

        private CompanyEntity GivenCompany()
        {
            return new CompanyEntity()
            {
                Id = 1,
                Name = "Company Test"
            };
        }
    }
}
