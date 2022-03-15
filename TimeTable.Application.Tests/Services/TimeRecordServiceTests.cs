using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TimeTable.Application.Constants;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services;
using TimeTable.Application.Exceptions;
using TimeTable.Application.Services;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;

namespace TimeTable.Application.Tests.Services
{
    [TestClass]
    public class TimeRecordServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<ITimeRecordRepository> timeRecordRepositoryMock;
        private readonly Mock<IAppConfig> appConfigMock;
        private readonly Mock<IUserService> userServiceMock;

        public TimeRecordServiceTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            timeRecordRepositoryMock = new Mock<ITimeRecordRepository>();
            appConfigMock = new Mock<IAppConfig>();
            userServiceMock = new Mock<IUserService>();
        }

        [TestMethod]
        public async Task GetAll_Ok()
        {
            int pageSize = 5;
            List<TimeRecordEntity> givenTimeRecordList = GivenTimeRecordList(pageSize);
            timeRecordRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(givenTimeRecordList);
            timeRecordRepositoryMock.Setup(x => x.GetTotalRecordsAsync()).ReturnsAsync(22);

            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            PaginatedResponse<ReadingTimeRecord> timeRecords = await timeRecordService.GetAllAsync(new PaginationRequest { PageSize = pageSize, PageNumber = 1 });

            Assert.AreEqual(22, timeRecords.TotalRegisters);
            Assert.AreEqual(pageSize, timeRecords.Result.Count());
        }

        [TestMethod]
        public async Task GetAllOwn_Ok()
        {
            int pageSize = 5;
            List<TimeRecordEntity> givenTimeRecordList = GivenTimeRecordList(pageSize);
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(1);
            timeRecordRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(givenTimeRecordList);
            timeRecordRepositoryMock.Setup(x => x.GetTotalRecordsAsync(It.IsAny<int>())).ReturnsAsync(22);

            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            PaginatedResponse<ReadingTimeRecord> timeRecords = await timeRecordService.GetAllOwnAsync(new PaginationRequest { PageSize = pageSize, PageNumber = 1 });

            Assert.AreEqual(22, timeRecords.TotalRegisters);
            Assert.AreEqual(pageSize, timeRecords.Result.Count());
        }

        [TestMethod]
        public async Task GetOk_Ok()
        {
            int timeRecordId = 1;
            TimeRecordEntity givenTimeRecord = GivenTimeRecordWithEndDatetime(timeRecordId);
            timeRecordRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(givenTimeRecord);

            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            ReadingTimeRecord timeRecord = await timeRecordService.GetAsync(timeRecordId);

            Assert.AreEqual(givenTimeRecord.Id, timeRecord.Id);
            Assert.AreEqual(givenTimeRecord.PersonId, timeRecord.PersonId);
            Assert.AreEqual(givenTimeRecord.StartDateTime, timeRecord.StartDateTime);
            Assert.AreEqual(givenTimeRecord.EndDateTime, timeRecord.EndDateTime);
        }

        [TestMethod]
        public async Task GetOwn_Ok()
        {
            int timeRecordId = 1;
            TimeRecordEntity givenTimeRecord = GivenTimeRecordWithEndDatetime(timeRecordId);
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(1);
            timeRecordRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(givenTimeRecord);

            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            ReadingTimeRecord timeRecord = await timeRecordService.GetOwnAsync(timeRecordId);

            Assert.AreEqual(givenTimeRecord.Id, timeRecord.Id);
            Assert.AreEqual(givenTimeRecord.PersonId, timeRecord.PersonId);
            Assert.AreEqual(givenTimeRecord.StartDateTime, timeRecord.StartDateTime);
            Assert.AreEqual(givenTimeRecord.EndDateTime, timeRecord.EndDateTime);
        }

        [TestMethod]
        public async Task Add_WithEndDateTimeOk()
        {
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(1);
            timeRecordRepositoryMock.Setup(x => x.AddAsync(It.IsAny<TimeRecordEntity>())).Returns(Task.CompletedTask);
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(It.IsAny<int>(), It.IsAny<DateTimeOffset>())).ReturnsAsync(false);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            CreatingTimeRecord timeRecord = new CreatingTimeRecord()
            {
                StartDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
                EndDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1).AddHours(7)),
            };
            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            int timeRecordId = await timeRecordService.AddAsync(timeRecord);

            Assert.IsInstanceOfType(timeRecordId, typeof(int));
        }

        [TestMethod]
        public async Task Add_WithoutEndDateTimeOk()
        {
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(1);
            timeRecordRepositoryMock.Setup(x => x.AddAsync(It.IsAny<TimeRecordEntity>())).Returns(Task.CompletedTask);
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(It.IsAny<int>(), It.IsAny<DateTimeOffset>())).ReturnsAsync(false);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            CreatingTimeRecord timeRecord = new CreatingTimeRecord()
            {
                StartDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
                EndDateTime = null,
            };
            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            int timeRecordId = await timeRecordService.AddAsync(timeRecord);

            Assert.IsInstanceOfType(timeRecordId, typeof(int));
        }

        [TestMethod]
        public async Task Add_StartDateTimeOverlappedError()
        {
            int personId = 1;
            CreatingTimeRecord timeRecord = new CreatingTimeRecord()
            {
                StartDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
                EndDateTime = null,
            };
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(personId);
            timeRecordRepositoryMock.Setup(x => x.AddAsync(It.IsAny<TimeRecordEntity>())).Returns(Task.CompletedTask);
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(0, timeRecord.StartDateTime)).ReturnsAsync(true);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            
            await Assert.ThrowsExceptionAsync<NotValidOperationException>(() => timeRecordService.AddAsync(timeRecord), ErrorCodes.TIME_RECORD_OVERLAPPING_EXISTS);
        }

        [TestMethod]
        public async Task Add_EndDateTimeOverlappedError()
        {
            int personId = 1;
            CreatingTimeRecord timeRecord = new CreatingTimeRecord()
            {
                StartDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
                EndDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1).AddHours(7)),
            };
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(personId);
            timeRecordRepositoryMock.Setup(x => x.AddAsync(It.IsAny<TimeRecordEntity>())).Returns(Task.CompletedTask);
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(0, timeRecord.StartDateTime)).ReturnsAsync(false);
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(0, timeRecord.EndDateTime.Value)).ReturnsAsync(true);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            
            await Assert.ThrowsExceptionAsync<NotValidOperationException>(() => timeRecordService.AddAsync(timeRecord), ErrorCodes.TIME_RECORD_OVERLAPPING_EXISTS);
        }

        [TestMethod]
        public void Update_WithEndDateTimeOk()
        {
            int personId = 1;
            UpdatingTimeRecord timeRecord = new UpdatingTimeRecord()
            {
                Id = 1,
                StartDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
                EndDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1).AddHours(7)),
                RowVersion = Array.Empty<byte>()
            };
            timeRecordRepositoryMock.Setup(x => x.GetAsync(timeRecord.Id)).ReturnsAsync(GivenTimeRecordWithEndDatetime(timeRecord.Id));
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(personId);
            timeRecordRepositoryMock.Setup(x => x.AttachAsync(It.IsAny<int>(), It.IsAny<byte[]>())).ReturnsAsync(new TimeRecordEntity { Id = timeRecord.Id, RowVersion = timeRecord .RowVersion});
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(timeRecord.Id, timeRecord.StartDateTime)).ReturnsAsync(false);
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(timeRecord.Id, timeRecord.EndDateTime.Value)).ReturnsAsync(false);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            Task result = timeRecordService.UpdateAsync(timeRecord);

            Assert.AreEqual(Task.CompletedTask, result);
        }

        [TestMethod]
        public void Update_WithoutEndDateTimeOk()
        {
            int personId = 1;
            UpdatingTimeRecord timeRecord = new UpdatingTimeRecord()
            {
                Id = 1,
                StartDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
                EndDateTime = null,
                RowVersion = Array.Empty<byte>()
            };
            timeRecordRepositoryMock.Setup(x => x.GetAsync(timeRecord.Id)).ReturnsAsync(GivenTimeRecordWithEndDatetime(timeRecord.Id));
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(personId);
            timeRecordRepositoryMock.Setup(x => x.AttachAsync(It.IsAny<int>(), It.IsAny<byte[]>())).ReturnsAsync(new TimeRecordEntity { Id = timeRecord.Id, RowVersion = timeRecord.RowVersion});
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(timeRecord.Id, timeRecord.StartDateTime)).ReturnsAsync(false);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            Task result = timeRecordService.UpdateAsync(timeRecord);

            Assert.AreEqual(Task.CompletedTask, result);
        }

        [TestMethod]
        public async Task Update_ForbidenError()
        {
            int personId = 1;
            UpdatingTimeRecord timeRecord = new UpdatingTimeRecord()
            {
                Id = 2,
                StartDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
                EndDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1).AddHours(7)),
                RowVersion = Array.Empty<byte>(),
            };
            timeRecordRepositoryMock.Setup(x => x.GetAsync(timeRecord.Id)).ReturnsAsync(GivenTimeRecordWithEndDatetime(timeRecord.Id));
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(personId);
            timeRecordRepositoryMock.Setup(x => x.AttachAsync(It.IsAny<int>(), It.IsAny<byte[]>())).ReturnsAsync(new TimeRecordEntity { Id = timeRecord.Id, RowVersion = timeRecord.RowVersion });
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(timeRecord.Id, timeRecord.StartDateTime)).ReturnsAsync(false);
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(timeRecord.Id, timeRecord.EndDateTime.Value)).ReturnsAsync(false);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            
            await Assert.ThrowsExceptionAsync<ForbidenActionException>(() => timeRecordService.UpdateAsync(timeRecord));
        }

        [TestMethod]
        public async Task Update_StartDateTimeOverlappedError()
        {
            int personId = 1;
            UpdatingTimeRecord timeRecord = new UpdatingTimeRecord()
            {
                Id = 1,
                StartDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
                EndDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1).AddHours(7)),
                RowVersion = Array.Empty<byte>(),
            };
            timeRecordRepositoryMock.Setup(x => x.GetAsync(timeRecord.Id)).ReturnsAsync(GivenTimeRecordWithEndDatetime(timeRecord.Id));
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(personId);
            timeRecordRepositoryMock.Setup(x => x.AttachAsync(It.IsAny<int>(), It.IsAny<byte[]>())).ReturnsAsync(new TimeRecordEntity { Id = timeRecord.Id, RowVersion = timeRecord.RowVersion });
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(timeRecord.Id, timeRecord.StartDateTime)).ReturnsAsync(true);
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(timeRecord.Id, timeRecord.EndDateTime.Value)).ReturnsAsync(false);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotValidOperationException>(() => timeRecordService.UpdateAsync(timeRecord), ErrorCodes.TIME_RECORD_OVERLAPPING_EXISTS);
        }

        [TestMethod]
        public async Task Update_EndDateTimeOverlappedError()
        {
            int personId = 1;
            UpdatingTimeRecord timeRecord = new UpdatingTimeRecord()
            {
                Id = 1,
                StartDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
                EndDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-1).AddHours(7)),
                RowVersion = Array.Empty<byte>(),
            };
            timeRecordRepositoryMock.Setup(x => x.GetAsync(timeRecord.Id)).ReturnsAsync(GivenTimeRecordWithEndDatetime(timeRecord.Id));
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(personId);
            timeRecordRepositoryMock.Setup(x => x.AttachAsync(It.IsAny<int>(), It.IsAny<byte[]>())).ReturnsAsync(new TimeRecordEntity { Id = timeRecord.Id, RowVersion = timeRecord.RowVersion });
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(timeRecord.Id, timeRecord.StartDateTime)).ReturnsAsync(false);
            timeRecordRepositoryMock.Setup(x => x.ExistsOverlappingAsync(timeRecord.Id, timeRecord.EndDateTime.Value)).ReturnsAsync(true);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);

            await Assert.ThrowsExceptionAsync<NotValidOperationException>(() => timeRecordService.UpdateAsync(timeRecord), ErrorCodes.TIME_RECORD_OVERLAPPING_EXISTS);
        }

        [TestMethod]
        public void Delete_Ok()
        {
            int personId = 1;
            int timeRecordId = 1;
            TimeRecordEntity givenTimeRecord = GivenTimeRecordWithEndDatetime(timeRecordId);
            timeRecordRepositoryMock.Setup(x => x.GetAsync(timeRecordId)).ReturnsAsync(givenTimeRecord);
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(personId);
            timeRecordRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<byte[]>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            Task result = timeRecordService.DeleteAsync(new DeleteRequest { Id = givenTimeRecord.Id, RowVersion = givenTimeRecord.RowVersion });
            
            Assert.AreEqual(Task.CompletedTask, result);
        }

        [TestMethod]
        public async Task Delete_ForbidenError()
        {
            int personId = 1;
            int timeRecordId = 2;
            var givenTimeRecord = GivenTimeRecordWithEndDatetime(timeRecordId);
            timeRecordRepositoryMock.Setup(x => x.GetAsync(timeRecordId)).ReturnsAsync(givenTimeRecord);
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(personId);
            timeRecordRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<byte[]>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            TimeRecordService timeRecordService = new TimeRecordService(unitOfWorkMock.Object, timeRecordRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ForbidenActionException>(() => timeRecordService.DeleteAsync(new DeleteRequest { Id = givenTimeRecord.Id, RowVersion = givenTimeRecord.RowVersion }));
        }

        private List<TimeRecordEntity> GivenTimeRecordList(int count)
        {
            Random rnd = new Random();

            var result = new List<TimeRecordEntity>();
            for (int i = 1; i <= count; i++)
            {
                Byte[] rndRowVersion = new Byte[8];
                rnd.NextBytes(rndRowVersion);
                var startDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-i));
                var endDateTime = startDateTime.AddHours(i % 8);

                result.Add(new TimeRecordEntity { Id = i, PersonId = i, StartDateTime = startDateTime, EndDateTime = endDateTime, RowVersion = rndRowVersion });
            }

            return result;
        }

        private TimeRecordEntity GivenTimeRecordWithEndDatetime(int timeRecordId)
        {
            Random rnd = new Random();
            Byte[] rndRowVersion = new Byte[8];
            rnd.NextBytes(rndRowVersion);

            var startDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-timeRecordId));
            var endDateTime = startDateTime.AddHours(timeRecordId % 8);
            
            return new TimeRecordEntity { Id = timeRecordId, PersonId = timeRecordId, StartDateTime = startDateTime, EndDateTime = endDateTime, RowVersion = rndRowVersion };
        }

        private TimeRecordEntity GivenTimeRecordWithoutEndDatetime(int timeRecordId)
        {
            Random rnd = new Random();
            Byte[] rndRowVersion = new Byte[8];
            rnd.NextBytes(rndRowVersion);
            var startDateTime = new DateTimeOffset(DateTime.UtcNow.AddDays(-timeRecordId));
            
            return new TimeRecordEntity { Id = timeRecordId, PersonId = timeRecordId, StartDateTime = startDateTime, EndDateTime = null, RowVersion = rndRowVersion };
        }
    }
}
