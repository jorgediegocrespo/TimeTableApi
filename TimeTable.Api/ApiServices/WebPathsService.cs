using Microsoft.AspNetCore.Hosting;
using TimeTable.Application.Contracts.Services;

namespace TimeTable.Api.ApiServices
{
    public class WebPathsService : IWebPathsService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public WebPathsService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public string GetWebRootPath()
        {
            return webHostEnvironment.WebRootPath;
        }
    }
}
