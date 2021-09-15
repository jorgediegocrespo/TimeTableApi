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
    [Route("api/person")]
    [ApiController]
    public class PersonController : BaseController<BasicReadingPerson, DetailedReadingPerson, CreationPerson, UpdatingBusinessPerson>
    {
        public PersonController(IPersonService service, IAppConfig config) : base(service, config)
        { }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(IEnumerable<BasicReadingPerson>), StatusCodes.Status200OK)]
        public override async Task<IActionResult> Get()
        {
            return await base.Get();
        }

        [HttpGet]
        [Route("items/{id}")]
        [ProducesResponseType(typeof(DetailedReadingPerson), StatusCodes.Status200OK)]
        public override async Task<IActionResult> Get(int id)
        {
            return await base.Get(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public override async Task<IActionResult> Post([FromBody] CreationPerson person)
        {
            return await base.Post(person);
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
