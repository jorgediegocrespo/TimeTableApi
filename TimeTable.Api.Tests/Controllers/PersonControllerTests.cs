using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeTable.Api.Tests.Controllers.Base;
using TimeTable.Application.Constants;
using TimeTable.Business.ConstantValues;
using TimeTable.Business.Models;
using TimeTable.DataAccess;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Api.Tests.Controllers
{
    [TestClass]
    public class PersonControllerTests : BaseControllerTest
    {
        private static readonly string url = "/api/person";

        [TestMethod]
        public async Task Get_Unauthorized()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            PaginationRequest pagination = new PaginationRequest { PageSize = 2, PageNumber = 1 };
            StringContent content = new StringContent(JsonConvert.SerializeObject(pagination), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/items", content);

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_NullBadRequest()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            StringContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/items", content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_PageSizeTooSortBadRequest()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            PaginationRequest pagination = new PaginationRequest { PageSize = 0, PageNumber = 1 };
            StringContent content = new StringContent(JsonConvert.SerializeObject(pagination), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/items", content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_PageSizeTooLongBadRequest()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            PaginationRequest pagination = new PaginationRequest { PageSize = 101, PageNumber = 1 };
            StringContent content = new StringContent(JsonConvert.SerializeObject(pagination), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/items", content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_Ok()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            PaginationRequest pagination = new PaginationRequest { PageSize = 2, PageNumber = 1 };
            StringContent content = new StringContent(JsonConvert.SerializeObject(pagination), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/items", content);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_Unauthorized()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync($"{url}/items/1");

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_NotFound()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync($"{url}/items/1456");

            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_Ok()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync($"{url}/items/1");

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetOwn_Unauthorized()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync($"{url}/ownItem");

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GetOwn_Ok()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync($"{url}/ownItem");

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_Unauthorized()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            CreatingPerson creatingPerson = new CreatingPerson { Name = "Test", Email = "test@test.com", Password = "1234_Test" };
            StringContent content = new StringContent(JsonConvert.SerializeObject(creatingPerson), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_NullBadRequest()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_NameBadRequest()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            CreatingPerson creatingPerson = new CreatingPerson { Name = "abc", Email = "test@test.com", Password = "1234_Test" };
            StringContent content = new StringContent(JsonConvert.SerializeObject(creatingPerson), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_EmailBadRequest()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            CreatingPerson creatingPerson = new CreatingPerson { Name = "Test", Email = "test", Password = "1234_Test" };
            StringContent content = new StringContent(JsonConvert.SerializeObject(creatingPerson), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_PasswordBadRequest()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            CreatingPerson creatingPerson = new CreatingPerson { Name = "Test", Email = "test@test.com", Password = null };
            StringContent content = new StringContent(JsonConvert.SerializeObject(creatingPerson), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_Ok()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            CreatingPerson creatingPerson = new CreatingPerson { Name = "Test", Email = "test@test.com", Password = "1234_Test" };
            StringContent content = new StringContent(JsonConvert.SerializeObject(creatingPerson), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            dynamic result = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);
            Assert.IsTrue(result.id > 0);
        }

        [TestMethod]
        public async Task Put_Unauthorized()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            HttpClient client = factory.CreateClient();

            UpdatingPerson updatingPerson = new UpdatingPerson { Id = Constants.EmployeeId, Name = "Employee updated" };
            StringContent content = new StringContent(JsonConvert.SerializeObject(updatingPerson), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_NullBadRequest()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_NameBadRequest()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            UpdatingPerson updatingPerson = new UpdatingPerson { Id = Constants.EmployeeId, Name = "123" };
            StringContent content = new StringContent(JsonConvert.SerializeObject(updatingPerson), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_Forbiden()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            UpdatingPerson updatingPerson = new UpdatingPerson { Id = 10, Name = "Test-Updated" };
            StringContent content = new StringContent(JsonConvert.SerializeObject(updatingPerson), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_Ok()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();
            
            UpdatingPerson updatingPerson = new UpdatingPerson { Id = Constants.EmployeeId, Name = "Employee updated" };
            StringContent content = new StringContent(JsonConvert.SerializeObject(updatingPerson), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_Unauthorized()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.DeleteAsync($"{url}/2");

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_Conflict()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.DeleteAsync($"{url}/1");
            CustomError result = JsonConvert.DeserializeObject<CustomError>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, response.StatusCode);
            Assert.AreEqual(ErrorCodes.PERSON_DEFAULT, result.error_code);
        }

        [TestMethod]
        public async Task Delete_Ok()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();
            TimeTableDbContext timeTableContext = BuildContext(dbContextName);
            await AddTestPerson(factory, timeTableContext);

            HttpResponseMessage response = await client.DeleteAsync($"{url}/10");

            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteOwn_Unauthorized()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.DeleteAsync($"{url}/own");

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteOwn_Conflict()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.DeleteAsync($"{url}/own");
            CustomError result = JsonConvert.DeserializeObject<CustomError>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, response.StatusCode);
            Assert.AreEqual(ErrorCodes.PERSON_DEFAULT, result.error_code);
        }

        [TestMethod]
        public async Task DeleteOwn_Ok()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            HttpResponseMessage response = await client.DeleteAsync($"{url}/own");

            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        private async Task AddTestPerson(WebApplicationFactory<Startup> factory, TimeTableDbContext context)
        {
            using (var scope = factory.Services.CreateScope())
            {
                IServiceProvider serviceProvider = scope.ServiceProvider;
                UserManager<IdentityUser> userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
                
                IdentityUser user = new IdentityUser
                {
                    Id = "76d0f070-2654-4279-af41-6b29739c5942",
                    UserName = "Test",
                    Email = "test@test.com",
                };

                await userManager.CreateAsync(user, Constants.AdminPassword);
                await userManager.AddToRoleAsync(user, RolesConsts.EMPLOYEE);

                string userId = await userManager.GetUserIdAsync(user);
                await context.People.AddAsync(new PersonEntity() { Id = 10, Name = "Test", UserId = userId, IsDefault = false });

                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();
            }
        }
    }
}
