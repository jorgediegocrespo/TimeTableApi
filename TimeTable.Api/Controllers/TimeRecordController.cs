using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/timeRecord")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TimeRecordController : BaseCrudController<BasicReadingTimeRecord, DetailedReadingTimeRecord, CreationTimeRecord, UpdatingTimeRecord>
    {
        public TimeRecordController(ITimeRecordService service, IAppConfig config) : base(service, config)
        { }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(IEnumerable<BasicReadingTimeRecord>), StatusCodes.Status200OK)]
        public override async Task<IActionResult> Get()
        {
            return await base.Get();
        }

        [HttpGet]
        [Route("items/{id}")]
        [ProducesResponseType(typeof(DetailedReadingTimeRecord), StatusCodes.Status200OK)]
        public override async Task<IActionResult> GetById(int id)
        {
            return await base.GetById(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public override async Task<IActionResult> Post([FromBody] CreationTimeRecord timeRecord)
        {
            return await base.Post(timeRecord);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public override async Task<IActionResult> Put([FromBody] UpdatingTimeRecord timeRecord)
        {
            return await base.Put(timeRecord);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }
    }
}
