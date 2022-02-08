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
    public class PersonRepositoryTests : BaseRepositoryTests
    {
        [TestMethod]
        public async Task Exists_TrueWithLowerCase()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            await timeTableContext.People.AddAsync(new PersonEntity { Id = 1, Name = "Person 1" });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            bool result = await personRepository.ExistsAsync(2, "person 1");

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task Exists_TrueWithUpperCase()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            await timeTableContext.People.AddAsync(new PersonEntity { Id = 1, Name = "Person 1" }); 
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            bool result = await personRepository.ExistsAsync(2, "PERSON 1");

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task Exists_TrueWithSpaces()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            await timeTableContext.People.AddAsync(new PersonEntity { Id = 1, Name = "Person 1" });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            bool result = await personRepository.ExistsAsync(2, " person 1  ");

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task Exists_FalseWithName()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            await timeTableContext.People.AddAsync(new PersonEntity { Id = 1, Name = "Person 1" });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            bool result = await personRepository.ExistsAsync(2, "Person 2");

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task Exists_FalseWithSameId()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            await timeTableContext.People.AddAsync(new PersonEntity { Id = 1, Name = "Person 1" });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            bool result = await personRepository.ExistsAsync(1, "Person 1");

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task GetTotalRecords_Ok()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            await timeTableContext.People.AddRangeAsync(new List<PersonEntity>
            {
                new PersonEntity { Id = 1, Name = "Person 1" },
                new PersonEntity { Id = 2, Name = "Person 2" },
                new PersonEntity { Id = 3, Name = "Person 3" },
                new PersonEntity { Id = 4, Name = "Person 4" },
                new PersonEntity { Id = 5, Name = "Person 5" },
                new PersonEntity { Id = 6, Name = "Person 6" },
                new PersonEntity { Id = 7, Name = "Person 7" }
            });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            int result = await personRepository.GetTotalRecordsAsync();

            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public async Task GetAll_FirstPageOk()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Id = 1, Name = "Person 1" },
                new PersonEntity { Id = 2, Name = "Person 3" },
                new PersonEntity { Id = 3, Name = "Person 5" },
                new PersonEntity { Id = 4, Name = "Person 7" },
                new PersonEntity { Id = 5, Name = "Person 6" },
                new PersonEntity { Id = 6, Name = "Person 4" },
                new PersonEntity { Id = 7, Name = "Person 2" }
            };
            await timeTableContext.People.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            var result = await personRepository.GetAllAsync(2, 1);
            var resultList = result.ToList();

            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual(7, resultList[1].Id);
        }

        [TestMethod]
        public async Task GetAll_SecondPageOk()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Id = 1, Name = "Person 1" },
                new PersonEntity { Id = 2, Name = "Person 3" },
                new PersonEntity { Id = 3, Name = "Person 5" },
                new PersonEntity { Id = 4, Name = "Person 7" },
                new PersonEntity { Id = 5, Name = "Person 6" },
                new PersonEntity { Id = 6, Name = "Person 4" },
                new PersonEntity { Id = 7, Name = "Person 2" }
            };
            await timeTableContext.People.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            var result = await personRepository.GetAllAsync(2, 2);
            var resultList = result.ToList();

            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(2, resultList[0].Id);
            Assert.AreEqual(6, resultList[1].Id);
        }

        [TestMethod]
        public async Task GetAll_LastPageOk()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Id = 1, Name = "Person 1" },
                new PersonEntity { Id = 2, Name = "Person 3" },
                new PersonEntity { Id = 3, Name = "Person 5" },
                new PersonEntity { Id = 4, Name = "Person 7" },
                new PersonEntity { Id = 5, Name = "Person 6" },
                new PersonEntity { Id = 6, Name = "Person 4" },
                new PersonEntity { Id = 7, Name = "Person 2" }
            };
            await timeTableContext.People.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            var result = await personRepository.GetAllAsync(2, 4);
            var resultList = result.ToList();

            Assert.AreEqual(1, resultList.Count);
            Assert.AreEqual(4, resultList[0].Id);
        }

        [TestMethod]
        public async Task Get_ByIdOk()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Id = 1, Name = "Person 1" },
                new PersonEntity { Id = 2, Name = "Person 3" },
                new PersonEntity { Id = 3, Name = "Person 5" },
                new PersonEntity { Id = 4, Name = "Person 7" },
                new PersonEntity { Id = 5, Name = "Person 6" },
                new PersonEntity { Id = 6, Name = "Person 4" },
                new PersonEntity { Id = 7, Name = "Person 2" }
            };
            await timeTableContext.People.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            var result = await personRepository.GetAsync(3);

            Assert.AreEqual(3, result.Id);
        }

        [TestMethod]
        public async Task Get_ByUserIdOk()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Id = 1, Name = "Person 1", UserId = "User1" },
                new PersonEntity { Id = 2, Name = "Person 3", UserId = "User3" },
                new PersonEntity { Id = 3, Name = "Person 5", UserId = "User5" },
                new PersonEntity { Id = 4, Name = "Person 7", UserId = "User7" },
                new PersonEntity { Id = 5, Name = "Person 6", UserId = "User6" },
                new PersonEntity { Id = 6, Name = "Person 4", UserId = "User4" },
                new PersonEntity { Id = 7, Name = "Person 2", UserId = "User2" }
            };
            await timeTableContext.People.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            var result = await personRepository.GetAsync("User5");

            Assert.AreEqual(3, result.Id);
        }

        [TestMethod]
        public async Task Add_Ok()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            await personRepository.AddAsync(new PersonEntity { Id = 1, Name = "Person 1" });
            await timeTableContext.SaveChangesAsync();

            timeTableContext.ChangeTracker.Clear();
            Assert.AreEqual(1, timeTableContext.People.Count());
        }

        [TestMethod]
        public async Task Update_Ok()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Id = 1, Name = "Person 1", UserId = "User1" },
                new PersonEntity { Id = 2, Name = "Person 3", UserId = "User3" },
                new PersonEntity { Id = 3, Name = "Person 5", UserId = "User5" },
                new PersonEntity { Id = 4, Name = "Person 7", UserId = "User7" },
                new PersonEntity { Id = 5, Name = "Person 6", UserId = "User6" },
                new PersonEntity { Id = 6, Name = "Person 4", UserId = "User4" },
                new PersonEntity { Id = 7, Name = "Person 2", UserId = "User2" }
            };
            var toUpdate = new PersonEntity { Id = 8, Name = "Person 8", UserId = "User8" };
            await timeTableContext.People.AddRangeAsync(sourceList);
            await timeTableContext.People.AddAsync(toUpdate);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            var entity = await personRepository.AttachAsync(toUpdate.Id, toUpdate.RowVersion);
            entity.Name = "Updated";
            await timeTableContext.SaveChangesAsync();

            timeTableContext.ChangeTracker.Clear();
            var validation = await timeTableContext.People.FirstAsync(x => x.Id == 8);
            Assert.AreEqual(validation.Name, "Updated");
        }

        [TestMethod]
        public async Task Delete_Ok()
        {
            TimeTableDbContext timeTableContext = GetInMemoryTimeTableDbContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Id = 1, Name = "Person 1", UserId = "User1" },
                new PersonEntity { Id = 2, Name = "Person 3", UserId = "User3" },
                new PersonEntity { Id = 3, Name = "Person 5", UserId = "User5" },
                new PersonEntity { Id = 4, Name = "Person 7", UserId = "User7" },
                new PersonEntity { Id = 5, Name = "Person 6", UserId = "User6" },
                new PersonEntity { Id = 6, Name = "Person 4", UserId = "User4" },
                new PersonEntity { Id = 7, Name = "Person 2", UserId = "User2" }
            };
            await timeTableContext.People.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            var toRemove = await timeTableContext.People.FirstOrDefaultAsync(x => x.Id == 4);
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            await personRepository.DeleteAsync(toRemove.Id, toRemove.RowVersion);
            await timeTableContext.SaveChangesAsync();

            timeTableContext.ChangeTracker.Clear();
            Assert.AreEqual(6, timeTableContext.People.Count());
        }
    }
}
