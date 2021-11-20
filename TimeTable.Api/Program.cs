using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using TimeTable.CrossCutting.Register;

namespace TimeTable.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().BuildContext().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
