﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    public class PersonController : BaseController
    {
        private readonly IPersonService service;
        private readonly IUserService userService;
        

        public PersonController(IPersonService service, IUserService userService, IAppConfig config) : base(config)
        {
            this.service = service;
            this.userService = userService;
        }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(IEnumerable<ReadingPerson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = RolesConsts.ADMIN)]
        public virtual async Task<IActionResult> Get()
        {
            var entities = await service.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet]
        [Route("items/{id}")]
        [ProducesResponseType(typeof(ReadingPerson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = RolesConsts.ADMIN)]
        public virtual async Task<IActionResult> GetById([FromQuery]int id)
        {
            var entity = await service.GetAsync(id);
            if (entity == null)
                return NotFound();
            else
                return Ok(entity);
        }

        [HttpGet]
        [Route("own")]
        [ProducesResponseType(typeof(ReadingPerson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOwn()
        {
            return Ok(await service.GetOwnAsync());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = RolesConsts.ADMIN)]
        public virtual async Task<IActionResult> Post([FromBody]CreatingPerson item)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> Put([FromBody]UpdatingPerson item)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = RolesConsts.ADMIN)]
        public virtual async Task<IActionResult> Delete([FromQuery]int id)
        {
            await service.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete]
        [Route("own")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOwn()
        {
            await service.DeleteOwnAsync();
            return NoContent();
        }
    }
}
