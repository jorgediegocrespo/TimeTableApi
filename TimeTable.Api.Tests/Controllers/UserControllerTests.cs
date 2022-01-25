using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeTable.Api.Tests.Controllers.Base;
using TimeTable.Business.Models;

namespace TimeTable.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests : BaseControllerTest
    {
        private static readonly string url = "/api/users";

        [TestMethod]
        public async Task Get_Ok()
        {
            WebApplicationFactory<Startup> factory = BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            HttpClient client = factory.CreateClient();

            LoginUserInfo loginUserInfo = new LoginUserInfo { UserName = Constants.AdminName, Password = Constants.AdminPassword };
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginUserInfo), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/login", content);
            var token = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(token));
        }

        [TestMethod]
        public async Task Get_NullBadRequest()
        {
            WebApplicationFactory<Startup> factory = BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            HttpClient client = factory.CreateClient();

            StringContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/login", content);
            
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_NoLoginBadRequest()
        {
            WebApplicationFactory<Startup> factory = BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            HttpClient client = factory.CreateClient();

            LoginUserInfo loginUserInfo = new LoginUserInfo { UserName = null, Password = Constants.AdminPassword };
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginUserInfo), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/login", content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_NoPasswordBadRequest()
        {
            WebApplicationFactory<Startup> factory = BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            HttpClient client = factory.CreateClient();

            LoginUserInfo loginUserInfo = new LoginUserInfo { UserName = Constants.AdminName, Password = null };
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginUserInfo), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/login", content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_InvalidLoginBadRequest()
        {
            WebApplicationFactory<Startup> factory = BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            HttpClient client = factory.CreateClient();

            LoginUserInfo loginUserInfo = new LoginUserInfo { UserName = Constants.AdminName, Password = Constants.EmployeePassword };
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginUserInfo), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/login", content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
