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
    [Route("api/timeRecord")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TimeRecordController : BaseController
    {
        private readonly ITimeRecordService service;
        private readonly IUserService userService;

        public TimeRecordController(ITimeRecordService service, IAppConfig config) : base(config)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("items")]
        [ProducesResponseType(typeof(PaginatedResponse<ReadingTimeRecord>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = RolesConsts.ADMIN)]
        public async Task<IActionResult> Get([FromBody] PaginationRequest request)
        {
            if (request == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var entities = await service.GetAllAsync(request);
            return Ok(entities);
        }

        [HttpPost]
        [Route("ownItems")]
        [ProducesResponseType(typeof(PaginatedResponse<ReadingTimeRecord>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOwn([FromBody] PaginationRequest request)
        {
            if (request == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var entities = await service.GetAllOwnAsync(request);
            return Ok(entities);
        }

        [HttpGet]
        [Route("items/{id}")]
        [ProducesResponseType(typeof(ReadingTimeRecord), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = RolesConsts.ADMIN)]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await service.GetAsync(id);
            if (entity == null)
                return NotFound();
            else
                return Ok(entity);
        }

        [HttpGet]
        [Route("ownItems/{id}")]
        [ProducesResponseType(typeof(ReadingTimeRecord), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOwnById(int id)
        {
            var entity = await service.GetOwnAsync(id);
            if (entity == null)
                return NotFound();
            else
                return Ok(entity);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CreatingTimeRecord item)
        {
            if (item == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            int createdId = await service.AddAsync(item);
            return Created(nameof(GetById), new { id = createdId });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put([FromBody] UpdatingTimeRecord item)
        {
            if (item == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            await service.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id, byte[] rowVersion)
        {
            await service.DeleteAsync(id, rowVersion);
            return NoContent();
        }
    }
}
