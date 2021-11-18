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
    [Route("api/company")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CompanyController : BaseCrudController<BasicReadingCompany, DetailedReadingCompany, CreationCompany, UpdatingCompany>
    {
        public CompanyController(ICompanyService service, IAppConfig config) : base(service, config)
        { }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(IEnumerable<BasicReadingCompany>), StatusCodes.Status200OK)]
        public override async Task<IActionResult> Get()
        {
            return await Task.FromResult(Forbid());
        }

        [HttpGet]
        [Route("items/{id}")]
        [ProducesResponseType(typeof(DetailedReadingCompany), StatusCodes.Status200OK)]
        public override async Task<IActionResult> GetById(int id)
        {
            return await base.GetById(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize(Policy = PolicyConsts.NO_ADMIN)]
        public override async Task<IActionResult> Post([FromBody] CreationCompany company)
        {
            return await base.Post(company);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize(Policy = PolicyConsts.ADMIN)]
        public override async Task<IActionResult> Put([FromBody] UpdatingCompany company)
        {
            return await base.Put(company);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyConsts.ADMIN)]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }
    }
}
