using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.Api.Controllers.Base;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services;
using TimeTable.Business.Models;

namespace TimeTable.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/timeRecord")]
    public class TimeRecordController : BaseController<TimeRecord>
    {
        public TimeRecordController(ITimeRecordService service, IAppConfig config) : base(service, config)
        { }

        [Produces("application/json", Type = typeof(IEnumerable<TimeRecord>))]
        [HttpGet]
        public override async Task<IActionResult> Get()
        {
            return await base.Get();
        }

        [Produces("application/json", Type = typeof(TimeRecord))]
        [HttpGet("{id}")]
        public override async Task<IActionResult> Get(int id)
        {
            return await base.Get(id);
        }

        [Produces("application/json", Type = typeof(TimeRecord))]
        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] TimeRecord timeRecord)
        {
            return await base.Post(timeRecord);
        }

        [Produces("application/json", Type = typeof(Company))]
        [HttpPut]
        public override async Task<IActionResult> Put([FromBody] TimeRecord timeRecord)
        {
            return await base.Put(timeRecord);
        }

        [Produces("application/json", Type = typeof(bool))]
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }
    }
}
