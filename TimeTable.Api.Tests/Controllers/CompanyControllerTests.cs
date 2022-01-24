using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeTable.Api.Tests.Controllers.Base;
using TimeTable.Business.ConstantValues;

namespace TimeTable.Api.Tests.Controllers
{
    [TestClass]
    public class CompanyControllerTests : BaseControllerTest
    {
        private static readonly string url = "/api/company";

        [TestMethod]
        public async Task Get_Unauthorized()
        {
            WebApplicationFactory<Startup> factory = BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            HttpClient client = factory.CreateClient();
            HttpResponseMessage response = await client.GetAsync($"{url}/item");

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_Ok()
        {
            WebApplicationFactory<Startup> factory = BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();
            HttpResponseMessage response = await client.GetAsync($"{url}/item");
            
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_Forbiden()
        {
            WebApplicationFactory<Startup> factory = BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            StringContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_BadRequest()
        {
            WebApplicationFactory<Startup> factory = BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            StringContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
