using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimeTable.Api.Config;
using TimeTable.CrossCutting.Middleware;
using TimeTable.CrossCutting.Register;

namespace TimeTable.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IocRegister.RegisterDbContext(services, Configuration.GetConnectionString("DataBaseConnection"));
            IocRegister.AddRegistration(services);
            services.AddControllers();
            SwaggerConfig.AddRegistration(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            SwaggerConfig.AddRegistration(app);
            app.UseHttpsRedirection();
            app.UseRouting();

            //app.UseAuthorization();

            app.UseMiddlewares();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
