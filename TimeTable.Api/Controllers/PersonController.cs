using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.Api.Controllers.Base;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services;
using TimeTable.Business.ConstantValues;
using TimeTable.Business.Models;

namespace TimeTable.Api.Controllers
{
    [Route("api/person")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonController : BaseCrudController<BasicReadingPerson, DetailedReadingPerson, CreationPerson, UpdatingBusinessPerson>
    {
        private readonly IPersonService personService;
        private readonly IUserService userService;
        

        public PersonController(IPersonService service, IUserService userService, IAppConfig config) : base(service, config)
        {
            personService = service;
            this.userService = userService;
        }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(IEnumerable<BasicReadingPerson>), StatusCodes.Status200OK)]
        [Authorize(Roles = RolesConsts.ADMIN)]
        public override Task<IActionResult> Get()
        {
            return base.Get();
        }

        [HttpGet]
        [Route("items/{id}")]
        [ProducesResponseType(typeof(DetailedReadingPerson), StatusCodes.Status200OK)]
        [Authorize(Roles = RolesConsts.ADMIN)]
        public override async Task<IActionResult> GetById(int id)
        {
            return await base.GetById(id);
        }

        [HttpGet]
        [Route("own")]
        [ProducesResponseType(typeof(DetailedReadingPerson), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOwn()
        {
            return Ok(await personService.GetOwnAsync());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize(Roles = RolesConsts.ADMIN)]
        public override async Task<IActionResult> Post([FromBody] CreationPerson person)
        {
            if (person == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            int createdId = await personService.AddAsync(person);
            string token = await userService.LoginAsync(new LoginUserInfo
            {
                UserName = person.Name,
                Password = person.Password
            });

            return Created(nameof(GetById), new { id = createdId, token = token });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public override async Task<IActionResult> Put([FromBody] UpdatingBusinessPerson person)
        {
            return await base.Put(person);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = RolesConsts.ADMIN)]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [HttpDelete]
        [Route("own")]
        public async Task<IActionResult> DeleteOwn()
        {
            await personService.DeleteOwnAsync();
            return NoContent();
        }
    }
}
