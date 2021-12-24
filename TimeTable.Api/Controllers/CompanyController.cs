using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TimeTable.Api.Controllers.Base;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services;
using TimeTable.Business.ConstantValues;
using TimeTable.Business.Models;

namespace TimeTable.Api.Controllers
{
    [Route("api/company")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CompanyController : BaseController
    {
        private readonly ICompanyService service;
        private readonly UserManager<IdentityUser> aux;

        public CompanyController(ICompanyService service, IAppConfig config, UserManager<IdentityUser> aux) : base(config)
        {
            this.service = service;
            this.aux = aux;
        }

        [HttpGet]
        [Route("item")]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var aux1 = HttpContext.User.IsInRole(RolesConsts.ADMIN);
            var aux2 = HttpContext.User.IsInRole("Admin");
            Company entity = await service.GetAsync();
            if (entity == null)
                return NotFound();
            else
                return Ok(entity);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = RolesConsts.ADMIN)]
        public virtual async Task<IActionResult> Put(Company item)
        {
            if (item == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            await service.UpdateAsync(item);
            return NoContent();
        }
    }
}
