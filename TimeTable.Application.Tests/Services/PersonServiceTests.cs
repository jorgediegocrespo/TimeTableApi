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
    public class PersonServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IPersonRepository> personRepositoryMock;
        private readonly Mock<IAppConfig> appConfigMock;
        private readonly Mock<IUserService> userServiceMock;

        public PersonServiceTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            personRepositoryMock = new Mock<IPersonRepository>();
            appConfigMock = new Mock<IAppConfig>();
            userServiceMock = new Mock<IUserService>();
        }

        [TestMethod]
        public async Task GetAll_Ok()
        {
            int pageSize = 5;
            List<PersonEntity> givenPeopleList = GivenPeopleList(pageSize);
            personRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(givenPeopleList);
            personRepositoryMock.Setup(x => x.GetTotalRecordsAsync()).ReturnsAsync(22);

            PersonService personService = new PersonService(unitOfWorkMock.Object, personRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            PaginatedResponse<ReadingPerson> people = await personService.GetAllAsync(new PaginationRequest { PageSize = pageSize, PageNumber = 1});

            Assert.AreEqual(22, people.TotalRegisters);
            Assert.AreEqual(pageSize, people.Result.Count());
        }

        [TestMethod]
        public async Task Get_Ok()
        {
            int personId = 1;
            PersonEntity givenPerson = GivenDefaultPerson(personId);
            personRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(givenPerson);

            PersonService personService = new PersonService(unitOfWorkMock.Object, personRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            ReadingPerson person = await personService.GetAsync(personId);

            Assert.AreEqual(givenPerson.Id, person.Id);
            Assert.AreEqual(givenPerson.Name, person.Name);
            Assert.AreEqual(givenPerson.IsDefault, person.IsDefault);
        }

        [TestMethod]
        public async Task GetOwn_Ok()
        {
            int personId = 2;
            PersonEntity givenPerson = GivenNonDefaultPerson(personId);
            personRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(givenPerson);
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(personId);

            PersonService personService = new PersonService(unitOfWorkMock.Object, personRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            ReadingPerson person = await personService.GetOwnAsync();

            Assert.AreEqual(givenPerson.Id, person.Id);
            Assert.AreEqual(givenPerson.Name, person.Name);
            Assert.AreEqual(givenPerson.IsDefault, person.IsDefault);
        }

        [TestMethod]
        public async Task Add_Ok()
        {
            Exception resultException = null;
            unitOfWorkMock.Setup(x => x.SaveChangesInTransactionAsync(It.IsAny<Func<Task<int>>>())).Callback<Func<Task<int>>>(async func =>
            {
                try { await func.Invoke(); }
                catch (Exception ex) { resultException = ex; }
            });
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            personRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            personRepositoryMock.Setup(x => x.AddAsync(It.IsAny<PersonEntity>())).Returns(Task.CompletedTask);

            CreatingPerson person = new CreatingPerson()
            {
                Name = "Person 1",
                Email = "person1@test.com",
                Password = "Person1_1234"
            };
            PersonService personService = new PersonService(unitOfWorkMock.Object, personRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            int personId = await personService.AddAsync(person);

            Assert.IsInstanceOfType(personId, typeof(int));
            Assert.AreEqual(null, resultException);
        }

        [TestMethod]
        public async Task Add_PersonNameExistsError()
        {
            Exception resultException = null;
            unitOfWorkMock.Setup(x => x.SaveChangesInTransactionAsync(It.IsAny<Func<Task<int>>>())).Callback<Func<Task<int>>>(async func =>
            {
                try { await func.Invoke(); }
                catch (Exception ex) { resultException = ex; }
            });
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            personRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            personRepositoryMock.Setup(x => x.AddAsync(It.IsAny<PersonEntity>())).Returns(Task.CompletedTask);

            CreatingPerson person = new CreatingPerson()
            {
                Name = "Person 1",
                Email = "person1@test.com",
                Password = "Person1_1234"
            };
            PersonService personService = new PersonService(unitOfWorkMock.Object, personRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            int result = await personService.AddAsync(person);

            Assert.AreEqual(0, result);
            Assert.IsInstanceOfType(resultException, typeof(NotValidOperationException));
            Assert.AreEqual(ErrorCodes.PERSON_NAME_EXISTS, resultException.Message);
        }

        [TestMethod]
        public void Update_Ok()
        {
            int personId = 2;
            PersonEntity givenPerson = GivenNonDefaultPerson(personId);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(personId);
            personRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(givenPerson);
            personRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            personRepositoryMock.Setup(x => x.AttachAsync(It.IsAny<int>(), It.IsAny<byte[]>())).ReturnsAsync(new PersonEntity { Id = givenPerson.Id, RowVersion = givenPerson.RowVersion});


            UpdatingPerson person = new UpdatingPerson()
            {
                Id = personId,
                Name = "Person updated",
                RowVersion = givenPerson.RowVersion
            };
            PersonService personService = new PersonService(unitOfWorkMock.Object, personRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            Task result = personService.UpdateAsync(person);

            Assert.AreEqual(Task.CompletedTask, result);
        }

        [TestMethod]
        public async Task Update_PersonNameExistsError()
        {
            int personId = 2;
            PersonEntity givenPerson = GivenNonDefaultPerson(personId);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(personId);
            personRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(givenPerson);
            personRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            personRepositoryMock.Setup(x => x.AttachAsync(It.IsAny<int>(), It.IsAny<byte[]>())).ReturnsAsync(new PersonEntity { Id = givenPerson.Id, RowVersion = givenPerson.RowVersion });


            UpdatingPerson person = new UpdatingPerson()
            {
                Id = personId,
                Name = "Person updated",
                RowVersion = givenPerson.RowVersion
            };
            PersonService personService = new PersonService(unitOfWorkMock.Object, personRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            
            await Assert.ThrowsExceptionAsync<NotValidOperationException>(() => personService.UpdateAsync(person), ErrorCodes.PERSON_NAME_EXISTS);
        }

        [TestMethod]
        public async Task Update_ForbidenError()
        {
            int personId = 2;
            PersonEntity givenPerson = GivenNonDefaultPerson(personId);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            userServiceMock.Setup(x => x.GetContextPersonIdAsync()).ReturnsAsync(1);
            personRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(givenPerson);
            personRepositoryMock.Setup(x => x.ExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            personRepositoryMock.Setup(x => x.AttachAsync(It.IsAny<int>(), It.IsAny<byte[]>())).ReturnsAsync(new PersonEntity { Id = givenPerson.Id, RowVersion = givenPerson.RowVersion });


            UpdatingPerson person = new UpdatingPerson()
            {
                Id = personId,
                Name = "Person updated",
                RowVersion = givenPerson.RowVersion,
            };
            PersonService personService = new PersonService(unitOfWorkMock.Object, personRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);

            await Assert.ThrowsExceptionAsync<ForbidenActionException>(() => personService.UpdateAsync(person));
        }

        [TestMethod]
        public async Task Delete_Ok()
        {
            Exception resultException = null;
            unitOfWorkMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<Task>>())).Callback<Func<Task>>(async func =>
            {
                try { await func.Invoke(); }
                catch (Exception ex) { resultException = ex; }
            });
            int personId = 2;
            PersonEntity givenPerson = GivenNonDefaultPerson(personId);
            personRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(givenPerson);
            personRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<byte[]>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            userServiceMock.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            PersonService personService = new PersonService(unitOfWorkMock.Object, personRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            await personService.DeleteAsync(givenPerson.Id, givenPerson.RowVersion);

            Assert.AreEqual(null, resultException);
        }

        [TestMethod]
        public async Task Delete_PersonDefaultError()
        {
            Exception resultException = null;
            unitOfWorkMock.Setup(x => x.ExecuteInTransactionAsync(It.IsAny<Func<Task>>())).Callback<Func<Task>>(async func =>
            {
                try { await func.Invoke(); }
                catch (Exception ex) { resultException = ex; }
            });
            int personId = 2;
            PersonEntity givenPerson = GivenDefaultPerson(personId);
            personRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(givenPerson);
            personRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<byte[]>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            userServiceMock.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            PersonService personService = new PersonService(unitOfWorkMock.Object, personRepositoryMock.Object, appConfigMock.Object, userServiceMock.Object);
            await personService.DeleteAsync(givenPerson.Id, givenPerson.RowVersion);

            Assert.IsInstanceOfType(resultException, typeof(NotValidOperationException));
            Assert.AreEqual(ErrorCodes.PERSON_DEFAULT, resultException.Message);
        }

        private List<PersonEntity> GivenPeopleList(int count)
        {
            var result = new List<PersonEntity> { new PersonEntity { Id = 1, Name = "Admin", IsDefault = true }};

            for (int i = 2; i <= count; i++)
                result.Add(new PersonEntity { Id = i, Name = $"Person {i}", IsDefault = false });

            return result;
        }

        private PersonEntity GivenNonDefaultPerson(int personId)
        {
            return new PersonEntity { Id = personId, Name = $"Person {personId}", IsDefault = false };
        }

        private PersonEntity GivenDefaultPerson(int personId)
        {
            return new PersonEntity { Id = personId, Name = $"Person {personId}", IsDefault = true };
        }
    }
}
