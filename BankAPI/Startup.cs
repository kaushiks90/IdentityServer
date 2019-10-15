using BankAPI.Installer;
using BankAPI.Models;
using BankAPI.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BankAPI
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
            services.AddAuthentication("Bearer").AddIdentityServerAuthentication(options =>
            {
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;
                options.ApiName = "bankOfDotNetApi";
            });
            services.AddDbContext<BankContext>(opts => opts.UseInMemoryDatabase("BankingDb"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            SwaggerInstaller.IInstallServices(Configuration,services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var swaggerOptions = new SwaggerSetup();
            Configuration.GetSection(nameof(SwaggerSetup)).Bind(swaggerOptions);

            app.UseSwagger(options => { options.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(options => { options.SwaggerEndpoint(swaggerOptions.UIEndPoint, swaggerOptions.Description); });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
