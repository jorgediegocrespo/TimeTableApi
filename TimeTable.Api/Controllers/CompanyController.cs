using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.Api.Controllers.Base;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services;
using TimeTable.Business.Models;

namespace TimeTable.Api.Controllers
{
    [Route("api/company")]
    //[ApiController]
    public class CompanyController : BaseController<Company>
    {
        public CompanyController(ICompanyService service, IAppConfig config) : base(service, config)
        { }

        [HttpGet]
        [Route("items")]
        public override async Task<IActionResult> Get()
        {
            return await base.Get();
        }

        [HttpGet]
        [Route("items/{id}")]
        public override async Task<IActionResult> Get(int id)
        {
            return await base.Get(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public override async Task<IActionResult> Post([FromBody] Company company)
        {
            if (ModelState.IsValid)
            {
                return await base.Post(company);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public override async Task<IActionResult> Put([FromBody] Company company)
        {
            return await base.Put(company);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }
    }
}
