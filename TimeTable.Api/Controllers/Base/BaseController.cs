using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services.Base;
using TimeTable.Business.Models.Base;

namespace TimeTable.Api.Controllers.Base
{
    public class BaseCrudController<BR, DR, C, U> : ControllerBase
        where BR : IBasicReadingBusinessModel
        where DR : IDetailedReadingBusinessModel
        where C : ICreationBusinessModel
        where U : IUpdatingBusinessModel
    {
        protected readonly IBaseCrudService<BR, DR, C, U> service;
        protected readonly IAppConfig config;

        public BaseCrudController(IBaseCrudService<BR, DR, C, U> service, IAppConfig config)
        {
            this.service = service;
            this.config = config;
        }

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Get()
        {
            IEnumerable<BR> entities = await service.GetAllAsync();
            return Ok(entities);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> GetById(int id)
        {
            DR entity = await service.GetAsync(id);
            if (entity == null)
                return NotFound();
            else
                return Ok(entity);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Post(C item)
        {
            if (item == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            int createdId = await service.AddAsync(item);
            return Created(nameof(GetById), new { id = createdId});
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Put(U item)
        {
            if (item == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            await service.UpdateAsync(item);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Delete(int id)
        {
            await service.DeleteAsync(id);
            return NoContent();
        }
    }
}
