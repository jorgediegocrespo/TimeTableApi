using Microsoft.AspNetCore.Identity;
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
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            var timeRecordToAdd = new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) };
            await timeTableContext.TimeRecords.AddAsync(timeRecordToAdd);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            bool result = await timeRecordRepository.ExistsOverlappingAsync(timeRecordToAdd.Id + 1, person1.Id, new DateTimeOffset(2022, 1, 2, 16, 0, 0, TimeSpan.FromHours(2)));

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task ExistsOverlapping_FalseWithEndDateNull()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            var timeRecordToAdd = new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null };
            await timeTableContext.TimeRecords.AddAsync(timeRecordToAdd);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            bool result = await timeRecordRepository.ExistsOverlappingAsync(timeRecordToAdd.Id + 1, person1.Id, new DateTimeOffset(2022, 1, 2, 10, 25, 40, TimeSpan.FromHours(0)));

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task ExistsOverlapping_FalseOutsideRange()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            var timeRecordToAdd = new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) };
            await timeTableContext.TimeRecords.AddAsync(timeRecordToAdd);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            bool result = await timeRecordRepository.ExistsOverlappingAsync(timeRecordToAdd.Id + 1, person1.Id, new DateTimeOffset(2022, 1, 3, 9, 0, 0, TimeSpan.FromHours(0)));

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task ExistsOverlapping_FalseSameId()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            var timeRecordToAdd = new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) };
            await timeTableContext.TimeRecords.AddAsync(timeRecordToAdd);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository personRepository = new TimeRecordRepository(timeTableContext);
            bool result = await personRepository.ExistsOverlappingAsync(timeRecordToAdd.Id, person1.Id, new DateTimeOffset(2022, 1, 2, 9, 0, 0, TimeSpan.FromHours(0)));

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task ExistsOverlapping_FalseDifferentPerson()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            var timeRecordToAdd = new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) };
            await timeTableContext.TimeRecords.AddAsync(timeRecordToAdd);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository personRepository = new TimeRecordRepository(timeTableContext);
            bool result = await personRepository.ExistsOverlappingAsync(timeRecordToAdd.Id + 1, person1.Id + 1, new DateTimeOffset(2022, 1, 2, 9, 0, 0, TimeSpan.FromHours(0)));

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task GetTotalRecords_ByPersonOk()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            PersonEntity person2 = new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") };

            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 5, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            int result = await timeRecordRepository.GetTotalRecordsAsync(person2.Id);

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public async Task GetAll_FirstPageOk()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            PersonEntity person2 = new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") };

            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 5, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
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
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            PersonEntity person2 = new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") };

            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 5, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAllAsync(2, 2);
            var resultList = result.ToList();

            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(6, resultList[0].Id);
            Assert.AreEqual(2, resultList[1].Id);
        }

        [TestMethod]
        public async Task GetAll_LastPageOk()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            PersonEntity person2 = new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") };

            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 5, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
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
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            PersonEntity person2 = new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") };

            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 5, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 6, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAllAsync(person1.Id, 2, 1);
            var resultList = result.ToList();

            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual(2, resultList[1].Id);
        }

        [TestMethod]
        public async Task GetAll_ByPersonSecondPageOk()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            PersonEntity person2 = new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") };

            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 5, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 6, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAllAsync(person1.Id, 2, 2);
            var resultList = result.ToList();

            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(3, resultList[0].Id);
            Assert.AreEqual(4, resultList[1].Id);
        }

        [TestMethod]
        public async Task GetAll_ByPersonLastPageOk()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            PersonEntity person2 = new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") };

            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 5, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 6, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null}
            };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAllAsync(person1.Id, 2, 3);
            var resultList = result.ToList();

            Assert.AreEqual(1, resultList.Count);
            Assert.AreEqual(5, resultList[0].Id);
        }

        [TestMethod]
        public async Task Get_ByIdOk()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            PersonEntity person2 = new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") };

            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
            };
            var timeRecordToGet = new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.TimeRecords.AddRangeAsync(timeRecordToGet);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAsync(timeRecordToGet.Id);

            Assert.AreEqual(timeRecordToGet.Id, result.Id);
            Assert.AreEqual(timeRecordToGet.StartDateTime, result.StartDateTime);
            Assert.AreEqual(timeRecordToGet.EndDateTime, result.EndDateTime);
        }

        [TestMethod]
        public async Task Get_ByPersonOk()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            PersonEntity person2 = new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") };

            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
            };
            var timeRecordToGet = new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.TimeRecords.AddRangeAsync(timeRecordToGet);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAsync(timeRecordToGet.Id, person2.Id);

            Assert.AreEqual(timeRecordToGet.Id, result.Id);
            Assert.AreEqual(timeRecordToGet.PersonId, result.PersonId);
        }

        [TestMethod]
        public async Task Get_ByPersonNoRecord()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            PersonEntity person2 = new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") };

            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
            };
            var timeRecordToAdd = new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.TimeRecords.AddRangeAsync(timeRecordToAdd);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var result = await timeRecordRepository.GetAsync(timeRecordToAdd.Id, timeRecordToAdd.PersonId + 1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Add_Ok()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            timeTableContext.ChangeTracker.Clear();

            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            await timeRecordRepository.AddAsync(new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) });
            await timeTableContext.SaveChangesAsync();

            timeTableContext.ChangeTracker.Clear();
            Assert.AreEqual(1, timeTableContext.TimeRecords.Count());
        }

        [TestMethod]
        public async Task Update_Ok()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            PersonEntity person2 = new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") };

            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
            };
            var timeRecordToUpdate = new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.TimeRecords.AddRangeAsync(timeRecordToUpdate);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            var entity = await timeRecordRepository.AttachAsync(timeRecordToUpdate.Id, timeRecordToUpdate.RowVersion);
            var newEndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0));
            entity.EndDateTime = newEndDateTime;
            await timeTableContext.SaveChangesAsync();

            timeTableContext.ChangeTracker.Clear();
            var validation = await timeTableContext.TimeRecords.FirstAsync(x => x.Id == 7);
            Assert.AreEqual(newEndDateTime, validation.EndDateTime);
        }

        [TestMethod]
        public async Task Delete_Ok()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            PersonEntity person1 = new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") };
            PersonEntity person2 = new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") };

            var sourceList = new List<TimeRecordEntity>
            {
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 4, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person1, StartDateTime = new DateTimeOffset(2022, 1, 5, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 2, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 2, 15, 0, 0, TimeSpan.FromHours(0)) },
                new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 3, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = new DateTimeOffset(2022, 1, 3, 15, 0, 0, TimeSpan.FromHours(0)) },
            };
            var timeRecordToRemove = new TimeRecordEntity { Person = person2, StartDateTime = new DateTimeOffset(2022, 1, 4, 8, 0, 0, TimeSpan.FromHours(0)), EndDateTime = null };
            await timeTableContext.TimeRecords.AddRangeAsync(sourceList);
            await timeTableContext.TimeRecords.AddRangeAsync(timeRecordToRemove);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            TimeRecordRepository timeRecordRepository = new TimeRecordRepository(timeTableContext);
            await timeRecordRepository.DeleteAsync(timeRecordToRemove.Id, timeRecordToRemove.RowVersion);
            await timeTableContext.SaveChangesAsync();

            timeTableContext.ChangeTracker.Clear();
            Assert.AreEqual(6, timeTableContext.TimeRecords.Count());
        }
    }
}
