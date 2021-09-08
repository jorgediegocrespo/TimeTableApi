using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services.Base;
using TimeTable.Business.Models.Base;

namespace TimeTable.Api.Controllers.Base
{
    public class BaseController<T> : Controller
        where T : IBaseBusinessModel
    {
        private readonly IBaseCrudService<T> _service;
        private readonly IAppConfig _config;

        public BaseController(IBaseCrudService<T> service, IAppConfig config)
        {
            _service = service;
            _config = config;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Get()
        {
            var entities = await _service.GetAllAsync();
            return Ok(entities.ToList());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Get(int id)
        {
            if (id < 1)
                return BadRequest("id should be greater than 0");
            var entity = await _service.GetAsync(id);
            if (entity == null)
                return NotFound();
            else
                return Ok(entity);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Post(T item)
        {
            if (item == null)
                return BadRequest();

            var created = await _service.AddAsync(item);
            return CreatedAtRoute(nameof(Get), new { id = created.Id}, created);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]
        public virtual async Task<IActionResult> Put(T item)
        {
            var updated = await _service.UpdateAsync(item);
            return Ok(updated);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return Ok(deleted);
        }
    }
}
