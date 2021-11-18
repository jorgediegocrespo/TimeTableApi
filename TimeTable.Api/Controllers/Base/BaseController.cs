using Microsoft.AspNetCore.Mvc;
using TimeTable.Application.Contracts.Configuration;

namespace TimeTable.Api.Controllers.Base
{
    public class BaseController : ControllerBase
    {
        protected readonly IAppConfig config;

        public BaseController(IAppConfig config)
        {
            this.config = config;
        }
    }
}
