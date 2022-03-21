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
    public class PersonRepositoryTests : BaseRepositoryTests
    {
        [TestMethod]
        public async Task Exists_TrueWithLowerCase()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            await timeTableContext.People.AddAsync(new PersonEntity { Name = "Person 1" });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            bool result = await personRepository.ExistsAsync(2, "person 1");

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task Exists_TrueWithUpperCase()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            await timeTableContext.People.AddAsync(new PersonEntity { Name = "Person 1" }); 
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            bool result = await personRepository.ExistsAsync(2, "PERSON 1");

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task Exists_TrueWithSpaces()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            await timeTableContext.People.AddAsync(new PersonEntity { Name = "Person 1" });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            bool result = await personRepository.ExistsAsync(2, " person 1  ");

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task Exists_FalseWithName()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            await timeTableContext.People.AddAsync(new PersonEntity { Name = "Person 1" });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            bool result = await personRepository.ExistsAsync(2, "Person 2");

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task Exists_FalseWithSameId()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            await timeTableContext.People.AddAsync(new PersonEntity { Name = "Person 1" });
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            bool result = await personRepository.ExistsAsync(1, "Person 1");

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task GetTotalRecords_Ok()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            await timeTableContext.People.AddRangeAsync(new List<PersonEntity>
            {
                new PersonEntity { Name = "Person 1" },
                new PersonEntity { Name = "Person 2" },
                new PersonEntity { Name = "Person 3" },
                new PersonEntity { Name = "Person 4" },
                new PersonEntity { Name = "Person 5" },
                new PersonEntity { Name = "Person 6" },
                new PersonEntity { Name = "Person 7" }
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
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Name = "Person 1" },
                new PersonEntity { Name = "Person 3" },
                new PersonEntity { Name = "Person 5" },
                new PersonEntity { Name = "Person 7" },
                new PersonEntity { Name = "Person 6" },
                new PersonEntity { Name = "Person 4" },
                new PersonEntity { Name = "Person 2" }
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
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Name = "Person 1" },
                new PersonEntity { Name = "Person 3" },
                new PersonEntity { Name = "Person 5" },
                new PersonEntity { Name = "Person 7" },
                new PersonEntity { Name = "Person 6" },
                new PersonEntity { Name = "Person 4" },
                new PersonEntity { Name = "Person 2" }
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
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Name = "Person 1" },
                new PersonEntity { Name = "Person 3" },
                new PersonEntity { Name = "Person 5" },
                new PersonEntity { Name = "Person 7" },
                new PersonEntity { Name = "Person 6" },
                new PersonEntity { Name = "Person 4" },
                new PersonEntity { Name = "Person 2" }
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
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Name = "Person 1" },
                new PersonEntity { Name = "Person 3" },
                new PersonEntity { Name = "Person 5", PictureUrl = "https://secure.gravatar.com/avatar/f8ee566613f4c3742cebd790647b2deb?s=160&d=identicon&r=g" },
                new PersonEntity { Name = "Person 7" },
                new PersonEntity { Name = "Person 6" },
                new PersonEntity { Name = "Person 4" },
                new PersonEntity { Name = "Person 2" }
            };
            await timeTableContext.People.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            var result = await personRepository.GetAsync(3);

            Assert.AreEqual(3, result.Id);
            Assert.AreEqual("Person 5", result.Name);
            Assert.AreEqual("https://secure.gravatar.com/avatar/f8ee566613f4c3742cebd790647b2deb?s=160&d=identicon&r=g", result.PictureUrl);
        }

        [TestMethod]
        public async Task Get_ByUserIdOk()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") },
                new PersonEntity { Name = "Person 3", User = new IdentityUser("user3") },
                new PersonEntity { Name = "Person 5", User = new IdentityUser("user5") },
                new PersonEntity { Name = "Person 7", User = new IdentityUser("user7") },
                new PersonEntity { Name = "Person 6", User = new IdentityUser("user6") },
                new PersonEntity { Name = "Person 4", User = new IdentityUser("user4") },
                new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") }
            };
            var toRead = new PersonEntity { Name = "Person 8", User = new IdentityUser("user8"), PictureUrl = "https://secure.gravatar.com/avatar/f8ee566613f4c3742cebd790647b2deb?s=160&d=identicon&r=g" };
            await timeTableContext.People.AddRangeAsync(sourceList);
            await timeTableContext.People.AddAsync(toRead);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            var result = await personRepository.GetAsync(toRead.UserId);

            Assert.AreEqual("Person 8", result.Name);
            Assert.AreEqual("https://secure.gravatar.com/avatar/f8ee566613f4c3742cebd790647b2deb?s=160&d=identicon&r=g", result.PictureUrl);
        }

        [TestMethod]
        public async Task Add_Ok()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            await personRepository.AddAsync(new PersonEntity { Name = "Person 1", PictureUrl = "https://secure.gravatar.com/avatar/f8ee566613f4c3742cebd790647b2deb?s=160&d=identicon&r=g" });
            await timeTableContext.SaveChangesAsync();

            timeTableContext.ChangeTracker.Clear();
            Assert.AreEqual(1, timeTableContext.People.Count());
        }

        [TestMethod]
        public async Task Update_Ok()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") },
                new PersonEntity { Name = "Person 3", User = new IdentityUser("user3") },
                new PersonEntity { Name = "Person 5", User = new IdentityUser("user5") },
                new PersonEntity { Name = "Person 7", User = new IdentityUser("user7") },
                new PersonEntity { Name = "Person 6", User = new IdentityUser("user6") },
                new PersonEntity { Name = "Person 4", User = new IdentityUser("user4") },
                new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") }
            };
            var toUpdate = new PersonEntity { Name = "Person 8", User = new IdentityUser("user8"), PictureUrl = "https://secure.gravatar.com/avatar/f8ee566613f4c3742cebd790647b2deb?s=160&d=identicon&r=g" };
            await timeTableContext.People.AddRangeAsync(sourceList);
            await timeTableContext.People.AddAsync(toUpdate);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            var entity = await personRepository.AttachAsync(toUpdate.Id, toUpdate.RowVersion);
            entity.Name = "Updated";
            entity.PictureUrl = "https://jorgediegocrespo.files.wordpress.com/2021/09/headeranimation.jpg";
            await timeTableContext.SaveChangesAsync();

            timeTableContext.ChangeTracker.Clear();
            var validation = await timeTableContext.People.FirstAsync(x => x.Id == 8);
            Assert.AreEqual("Updated", validation.Name);
            Assert.AreEqual("https://jorgediegocrespo.files.wordpress.com/2021/09/headeranimation.jpg", validation.PictureUrl);
        }

        [TestMethod]
        public async Task Delete_Ok()
        {
            TimeTableDbContext timeTableContext = await GetLocalDbTimeTableContext(Guid.NewGuid().ToString());
            var sourceList = new List<PersonEntity>
            {
                new PersonEntity { Name = "Person 1", User = new IdentityUser("user1") },
                new PersonEntity { Name = "Person 3", User = new IdentityUser("user3") },
                new PersonEntity { Name = "Person 5", User = new IdentityUser("user5") },
                new PersonEntity { Name = "Person 7", User = new IdentityUser("user7") },
                new PersonEntity { Name = "Person 6", User = new IdentityUser("user6") },
                new PersonEntity { Name = "Person 4", User = new IdentityUser("user4") },
                new PersonEntity { Name = "Person 2", User = new IdentityUser("user2") }
            };
            await timeTableContext.People.AddRangeAsync(sourceList);
            await timeTableContext.SaveChangesAsync();
            timeTableContext.ChangeTracker.Clear();

            var toRemove = await timeTableContext.People.FirstOrDefaultAsync();
            timeTableContext.ChangeTracker.Clear();

            PersonRepository personRepository = new PersonRepository(timeTableContext);
            await personRepository.DeleteAsync(toRemove.Id, toRemove.RowVersion);
            await timeTableContext.SaveChangesAsync();

            timeTableContext.ChangeTracker.Clear();
            Assert.AreEqual(6, timeTableContext.People.Count());
        }
    }
}
