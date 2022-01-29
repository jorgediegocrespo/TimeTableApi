using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Repositories;
using TimeTable.DataAccess.Tests.Repositories.Base;

namespace TimeTable.DataAccess.Tests.Repositories
{
    [TestClass]
    public class TimeRecordRepositoryTests : BaseRepositoryTests
    {
        [TestMethod]
        public async Task ExistsOverlapping_True()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            await timeTableContext.TimeRecords.AddAsync(new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            bool result = await timeRecordRepository.ExistsOverlappingAsync(2, new DateTimeOffset(2022, 1, 2, 10, 25, 40, TimeSpan.FromHours(0)));

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task ExistsOverlapping_FalseWithEndDateNull()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            await timeTableContext.TimeRecords.AddAsync(new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            bool result = await timeRecordRepository.ExistsOverlappingAsync(2, new DateTimeOffset(2022, 1, 2, 10, 25, 40, TimeSpan.FromHours(0)));

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task ExistsOverlapping_FalseOutsideRange()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            await timeTableContext.TimeRecords.AddAsync(new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            bool result = await timeRecordRepository.ExistsOverlappingAsync(2, new DateTimeOffset(2022, 1, 3, 9, 0, 0, TimeSpan.FromHours(0)));

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task ExistsOverlapping_FalseSameId()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            await timeTableContext.TimeRecords.AddAsync(new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository personRepository = new TimeRecordRepository(timeTableContext);
            bool result = await personRepository.ExistsOverlappingAsync(1, new DateTimeOffset(2022, 1, 2, 9, 0, 0, TimeSpan.FromHours(0)));

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task GetTotalRecords_ByPersonOk()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            await timeTableContext.TimeRecords.AddRangeAsync(new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 2, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 3, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 4, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Id = 5, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 6, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 7, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            int result = await timeRecordRepository.GetTotalRecordsAsync(2);

            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public async Task GetAll_FirstPageOk()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 2, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 3, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 4, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Id = 5, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 6, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 7, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAllAsync(2, 1);
            var resultList = result.ToList();

            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual(5, resultList[1].Id);
        }

        [TestMethod]
        public async Task GetAll_SecondPageOk()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 2, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 3, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 4, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Id = 5, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 6, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 7, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAllAsync(2, 2);
            var resultList = result.ToList();

            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(2, resultList[0].Id);
            Assert.AreEqual(6, resultList[1].Id);
        }

        [TestMethod]
        public async Task GetAll_LastPageOk()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 2, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 3, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 4, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Id = 5, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 6, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 7, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAllAsync(2, 4);
            var resultList = result.ToList();

            Assert.AreEqual(1, resultList.Count);
            Assert.AreEqual(4, resultList[0].Id);
        }

        [TestMethod]
        public async Task GetAll_ByPersonFirstPageOk()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 3, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 5, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 7, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 5, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 8, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 6, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Id = 2, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 4, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 6, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAllAsync(2, 2, 1);
            var resultList = result.ToList();

            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual(3, resultList[1].Id);
        }

        [TestMethod]
        public async Task GetAll_ByPersonSecondPageOk()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 3, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 5, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 7, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 5, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 8, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 6, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Id = 2, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 4, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 6, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAllAsync(2, 2, 2);
            var resultList = result.ToList();

            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(5, resultList[0].Id);
            Assert.AreEqual(7, resultList[1].Id);
        }

        [TestMethod]
        public async Task GetAll_ByPersonLastPageOk()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 3, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 5, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 7, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 5, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 8, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 6, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Id = 2, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 4, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 6, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAllAsync(2, 2, 3);
            var resultList = result.ToList();

            Assert.AreEqual(1, resultList.Count);
            Assert.AreEqual(8, resultList[0].Id);
        }

        [TestMethod]
        public async Task Get_ByIdOk()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 2, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 3, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 4, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Id = 5, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 6, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 7, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAsync(3);

            Assert.AreEqual(3, result.Id);
        }

        [TestMethod]
        public async Task Get_ByPersonOk()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 2, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 3, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 4, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Id = 5, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 6, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 7, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAsync(3, 2);

            Assert.AreEqual(3, result.Id);
        }

        [TestMethod]
        public async Task Get_ByPersonNoRecord()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 2, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 3, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 4, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Id = 5, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 6, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 7, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAsync(3, 3);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Add_Ok()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            await timeRecordRepository.AddAsync(new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) });
            await timeTableContext.SaveChangesAsync();

            timeTableContext.ChangeTracker.Clear();
            Assert.AreEqual(1, timeTableContext.TimeRecords.Count());
        }

        [TestMethod]
        public async Task Update_Ok()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 2, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 3, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 4, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Id = 5, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 6, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) }
            };
            var toUpdate = new TimeRecordEntity { Id = 7, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.TimeRecords.AddAsync(toUpdate);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var newEndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0));
            toUpdate.EndDateTime = newEndDateTime;
            await timeRecordRepository.UpdateAsync(toUpdate);
            await timeTableContext.SaveChangesAsync();

            timeTableContext.ChangeTracker.Clear();
            var validation = await timeTableContext.TimeRecords.FirstAsync(x => x.Id == 7);
            Assert.AreEqual(newEndDateTime, validation.EndDateTime);
        }

        [TestMethod]
        public async Task Delete_Ok()
        {
            TimeTableDbContext timeTableContext = GetTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Id = 1, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 2, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 3, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 4, PersonId = 2, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Id = 5, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 6, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Id = 7, PersonId = 3, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            await timeRecordRepository.DeleteAsync(4);
            await timeTableContext.SaveChangesAsync();

            timeTableContext.ChangeTracker.Clear();
            Assert.AreEqual(6, timeTableContext.TimeRecords.Count());
        }
    }
}
