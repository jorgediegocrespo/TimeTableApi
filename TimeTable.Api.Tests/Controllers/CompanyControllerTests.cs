using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeTable.Api.Tests.Controllers.Base;
using TimeTable.Business.ConstantValues;
using TimeTable.Business.Models;
using TimeTable.DataAccess;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Api.Tests.Controllers
{
    [TestClass]
    public class CompanyControllerTests : BaseControllerTest
    {
        private static readonly string url = "/api/company";

        [TestMethod]
        public async Task Get_Unauthorized()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);

            using HttpClient client = factory.CreateClient();
            using HttpResponseMessage response = await client.GetAsync($"{url}/item");

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_Ok()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            using HttpResponseMessage response = await client.GetAsync($"{url}/item");
            var result = JsonConvert.DeserializeObject<Company>(await response.Content.ReadAsStringAsync());
            
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Put_Unauthorized()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            using HttpClient client = factory.CreateClient();

            using StringContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_NullBadRequest()
        {
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();

            using StringContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_NoNameBadRequest()
        {
            string dbContextName = Guid.NewGuid().ToString();
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();
            using TimeTableDbContext timeTableContext = BuildContext(dbContextName);
            CompanyEntity companyEntity = timeTableContext.Companies.First();
            timeTableContext.ChangeTracker.Clear();

            Company companyToUpdate = new Company { Id = companyEntity.Id, Name = string.Empty };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(companyToUpdate), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_TooSortNameBadRequest()
        {
            string dbContextName = Guid.NewGuid().ToString();
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();
            using TimeTableDbContext timeTableContext = BuildContext(dbContextName);
            CompanyEntity companyEntity = timeTableContext.Companies.First();
            timeTableContext.ChangeTracker.Clear();

            Company companyToUpdate = new Company { Id = companyEntity.Id, Name = "123" };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(companyToUpdate), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_TooLongNameBadRequest()
        {
            string dbContextName = Guid.NewGuid().ToString();
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();
            using TimeTableDbContext timeTableContext = BuildContext(dbContextName);
            CompanyEntity companyEntity = timeTableContext.Companies.First();
            timeTableContext.ChangeTracker.Clear();

            Company companyToUpdate = new Company { Id = companyEntity.Id, Name = "Etlaboresuscipitametinviduntsedelitametloremclitaameteirmodnoduoiriureipsumetrebumdoloresseavulputatevoluptuaetvelduiseraterossitstetdoloresquisaliquamkasddolorpraesentiustodoloresinviduntgubergrenautemnonumyutdolorplaceratametametnonumydolorvoluptuaconsequatsedloremmagnamagnaloremdiamclitaetnonummydoloreipsumameteirmodtemporgubergrencongueplaceratclitaduotakimatainviduntutloremdiamclitaloremvelloremconsectetuerextakimataullamcorperaccusamametvolu" };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(companyToUpdate), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_Ok()
        {
            string dbContextName = Guid.NewGuid().ToString();
            using WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.ADMIN);
            using HttpClient client = factory.CreateClient();
            using TimeTableDbContext timeTableContext = BuildContext(dbContextName);
            CompanyEntity companyEntity = timeTableContext.Companies.First();
            timeTableContext.ChangeTracker.Clear();

            Company companyToUpdate = new Company { Id = companyEntity.Id, Name = "Company Updated", RowVersion = companyEntity.RowVersion };
            using StringContent content = new StringContent(JsonConvert.SerializeObject(companyToUpdate), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync(url, content);
            
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
            CompanyEntity companyUpdated = timeTableContext.Companies.First();
            timeTableContext.ChangeTracker.Clear();
            Assert.AreEqual(companyToUpdate.Name, companyUpdated.Name);
        }
    }
}
