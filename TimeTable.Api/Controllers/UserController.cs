using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TimeTable.Api.Attributes;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services;
using TimeTable.Business.Models;

namespace TimeTable.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    [ApiKey]
    public class UserController : ControllerBase
    {
        private readonly IUserService service;

        public UserController(IUserService service, IAppConfig config)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginUserInfo userInfo)
        {
            if (userInfo == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            string token = await service.LoginAsync(userInfo);
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest();
            else
                return Ok(token);
        }
    }
}
