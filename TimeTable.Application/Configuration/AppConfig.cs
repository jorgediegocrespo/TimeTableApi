using Microsoft.Extensions.Configuration;
using TimeTable.Application.Contracts.Configuration;

namespace TimeTable.Application.Configuration
{
    public class AppConfig : IAppConfig
    {
        private readonly IConfiguration configuracion;

        public AppConfig(IConfiguration configuration)
        {
            configuracion = configuration;
        }

        public int MaxTrys => int.Parse(configuracion.GetSection("Polly:MaxTrys").Value);
        public int SecondToWait => int.Parse(configuracion.GetSection("Polly:TimeDelay").Value);
    }
}
