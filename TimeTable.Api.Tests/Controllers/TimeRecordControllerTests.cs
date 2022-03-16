using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
    public class TimeRecordControllerTests : BaseControllerTest
    {
        private static readonly string url = "/api/timeRecord";

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
        public async Task GetOwn_Unauthorized()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            HttpClient client = factory.CreateClient();

            PaginationRequest pagination = new PaginationRequest { PageSize = 2, PageNumber = 1 };
            StringContent content = new StringContent(JsonConvert.SerializeObject(pagination), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/ownItems", content);

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GetOwn_NullBadRequest()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            StringContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/ownItems", content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GetOwn_PageSizeTooSortBadRequest()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            PaginationRequest pagination = new PaginationRequest { PageSize = 0, PageNumber = 1 };
            StringContent content = new StringContent(JsonConvert.SerializeObject(pagination), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/ownItems", content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GetOwn_PageSizeTooLongBadRequest()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            PaginationRequest pagination = new PaginationRequest { PageSize = 101, PageNumber = 1 };
            StringContent content = new StringContent(JsonConvert.SerializeObject(pagination), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/ownItems", content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GetOwn_Ok()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            PaginationRequest pagination = new PaginationRequest { PageSize = 2, PageNumber = 1 };
            StringContent content = new StringContent(JsonConvert.SerializeObject(pagination), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{url}/ownItems", content);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_Unauthorized()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();
            await AddTestTimeRecordsAsync(dbContextName);

            HttpResponseMessage response = await client.GetAsync($"{url}/items/1");

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_NotFound()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();
            await AddTestTimeRecordsAsync(dbContextName);

            HttpResponseMessage response = await client.GetAsync($"{url}/items/1456");

            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_Ok()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.ADMIN);
            HttpClient client = factory.CreateClient();
            await AddTestTimeRecordsAsync(dbContextName);

            HttpResponseMessage response = await client.GetAsync($"{url}/items/1");

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetOwnById_Unauthorized()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, null);
            HttpClient client = factory.CreateClient();
            await AddTestTimeRecordsAsync(dbContextName);

            HttpResponseMessage response = await client.GetAsync($"{url}/ownItems/1");

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GetOwnById_ExistsButNotFound()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();
            await AddTestTimeRecordsAsync(dbContextName);

            HttpResponseMessage response = await client.GetAsync($"{url}/ownItems/1");

            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetOwnById_NotFound()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();
            await AddTestTimeRecordsAsync(dbContextName);

            HttpResponseMessage response = await client.GetAsync($"{url}/ownItems/1456");

            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetOwnById_Ok()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();
            await AddTestTimeRecordsAsync(dbContextName);

            HttpResponseMessage response = await client.GetAsync($"{url}/ownItems/2");

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_Unauthorized()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), null);
            HttpClient client = factory.CreateClient();

            CreatingTimeRecord creatingTimeRecord = new CreatingTimeRecord
            {
                StartDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 8, 0, 0)),
                EndDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 15, 0, 0))
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(creatingTimeRecord), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_NullBadRequest()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_Conflict()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();
            TimeTableDbContext timeTableContext = BuildContext(dbContextName);
            await timeTableContext.TimeRecords.AddAsync(new TimeRecordEntity()
            {
                PersonId = PeopleInfo.EmployeeId,
                StartDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 8, 0, 0)),
                EndDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 15, 0, 0))
            });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            CreatingTimeRecord creatingTimeRecord = new CreatingTimeRecord
            {
                StartDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 9, 0, 0)),
                EndDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 16, 0, 0))
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(creatingTimeRecord), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            dynamic result = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, response.StatusCode);
        }

        [TestMethod]
        public async Task Post_Ok()
        {
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(Guid.NewGuid().ToString(), RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();

            CreatingTimeRecord creatingTimeRecord = new CreatingTimeRecord
            {
                StartDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
                EndDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1).AddHours(7))
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(creatingTimeRecord), Encoding.UTF8, "application/json");
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

            StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");
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
        public async Task Put_Forbiden()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();
            TimeTableDbContext timeTableContext = BuildContext(dbContextName);
            var timeRecord = new TimeRecordEntity()
            {
                PersonId = PeopleInfo.AdminId,
                StartDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 8, 0, 0)),
                EndDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 15, 0, 0))
            };
            await timeTableContext.TimeRecords.AddAsync(timeRecord);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            UpdatingTimeRecord updatingTimeRecord = new UpdatingTimeRecord
            {
                Id = timeRecord.Id,
                StartDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 9, 0, 0)),
                EndDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 14, 0, 0)),
                RowVersion = timeRecord.RowVersion
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(updatingTimeRecord), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_Conflict()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();
            TimeTableDbContext timeTableContext = BuildContext(dbContextName);
            var timeRecord1 = new TimeRecordEntity()
            {
                PersonId = PeopleInfo.EmployeeId,
                StartDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 8, 0, 0)),
                EndDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 15, 0, 0))
            };
            var timeRecord2 = new TimeRecordEntity()
            {
                PersonId = PeopleInfo.EmployeeId,
                StartDateTime = new DateTimeOffset(new DateTime(2022, 1, 2, 8, 0, 0)),
                EndDateTime = new DateTimeOffset(new DateTime(2022, 1, 2, 15, 0, 0))
            };
            await timeTableContext.TimeRecords.AddAsync(timeRecord1);
            await timeTableContext.TimeRecords.AddAsync(timeRecord2);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            UpdatingTimeRecord updatingTimeRecord = new UpdatingTimeRecord
            {
                Id = timeRecord1.Id,
                StartDateTime = new DateTimeOffset(new DateTime(2022, 1, 2, 9, 0, 0)),
                EndDateTime = new DateTimeOffset(new DateTime(2022, 1, 2, 14, 0, 0)),
                RowVersion = timeRecord1.RowVersion,
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(updatingTimeRecord), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.Conflict, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_Ok()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();
            TimeTableDbContext timeTableContext = BuildContext(dbContextName);
            var timeRecord1 = new TimeRecordEntity()
            {
                PersonId = PeopleInfo.EmployeeId,
                StartDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 8, 0, 0)),
                EndDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 15, 0, 0))
            };
            await timeTableContext.TimeRecords.AddAsync(timeRecord1);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            UpdatingTimeRecord updatingTimeRecord = new UpdatingTimeRecord
            {
                Id = timeRecord1.Id,
                StartDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 9, 0, 0)),
                EndDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 16, 0, 0)),
                RowVersion = timeRecord1.RowVersion
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(updatingTimeRecord), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_Unauthorized()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, null);
            HttpClient client = factory.CreateClient();
            TimeTableDbContext timeTableContext = BuildContext(dbContextName);
            var timeRecord = new TimeRecordEntity()
            {
                PersonId = PeopleInfo.EmployeeId,
                StartDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 8, 0, 0)),
                EndDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 15, 0, 0))
            };
            await timeTableContext.TimeRecords.AddAsync(timeRecord);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            DeleteRequest deleteRequest = new DeleteRequest { Id = timeRecord.Id, RowVersion = timeRecord.RowVersion };
            StringContent content = new StringContent(JsonConvert.SerializeObject(deleteRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"{url}/delete", content);

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_Forbiden()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();
            TimeTableDbContext timeTableContext = BuildContext(dbContextName);
            var timeRecord = new TimeRecordEntity()
            {
                PersonId = PeopleInfo.AdminId,
                StartDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 8, 0, 0)),
                EndDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 15, 0, 0))
            };
            await timeTableContext.TimeRecords.AddAsync(timeRecord);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            DeleteRequest deleteRequest = new DeleteRequest { Id = timeRecord.Id, RowVersion = timeRecord.RowVersion };
            StringContent content = new StringContent(JsonConvert.SerializeObject(deleteRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"{url}/delete", content);

            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_Ok()
        {
            string dbContextName = Guid.NewGuid().ToString();
            WebApplicationFactory<Startup> factory = await BuildWebApplicationFactory(dbContextName, RolesConsts.EMPLOYEE);
            HttpClient client = factory.CreateClient();
            TimeTableDbContext timeTableContext = BuildContext(dbContextName);
            var timeRecord = new TimeRecordEntity()
            {
                PersonId = PeopleInfo.EmployeeId,
                StartDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 8, 0, 0)),
                EndDateTime = new DateTimeOffset(new DateTime(2022, 1, 1, 15, 0, 0))
            };
            await timeTableContext.TimeRecords.AddAsync(timeRecord);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            DeleteRequest deleteRequest = new DeleteRequest { Id = timeRecord.Id, RowVersion = timeRecord.RowVersion };
            StringContent content = new StringContent(JsonConvert.SerializeObject(deleteRequest), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"{url}/delete", content);

            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        private async Task AddTestTimeRecordsAsync(string dbContextName)
        {
            TimeTableDbContext timeTableContext = BuildContext(dbContextName);

            for (int i = 1; i <= 20; i++)
            {
                await timeTableContext.TimeRecords.AddAsync(new TimeRecordEntity()
                {
                    PersonId = (i % 2) == 0 ? PeopleInfo.EmployeeId : PeopleInfo.AdminId,
                    StartDateTime = new DateTimeOffset(new DateTime(2022,1,i,8,0,0)),
                    EndDateTime = new DateTimeOffset(new DateTime(2022, 1, i, 15, 0, 0))
                });
            }
            
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();
        }
    }
}
