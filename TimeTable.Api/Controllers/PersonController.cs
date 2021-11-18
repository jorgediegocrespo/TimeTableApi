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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = PolicyConsts.ADMIN)]
        public override async Task<IActionResult> Get()
        {
            return await Task.FromResult(Forbid());
        }

        [HttpGet]
        [Route("itemsByCompany/{companyId}")]
        [ProducesResponseType(typeof(DetailedReadingPerson), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = PolicyConsts.ADMIN)]
        public async Task<IActionResult> GetByCompanyId(int companyId)
        {
            var entities = await personService.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet]
        [Route("items/{id}")]
        [ProducesResponseType(typeof(DetailedReadingPerson), StatusCodes.Status200OK)]
        public override async Task<IActionResult> GetById(int id)
        {
            return await base.GetById(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [AllowAnonymous]
        public override async Task<IActionResult> Post([FromBody] CreationPerson person)
        {
            if (person == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            int createdId = await personService.AddAsync(person);
            string token = await userService.LoginAsync(new UserInfo
            {
                Email = person.Email,
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

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }
    }
}
