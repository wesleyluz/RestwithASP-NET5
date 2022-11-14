using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
//Model
using RestWithASPNET.Model.Context;
//Business
using RestWithASPNET.Business;
using RestWithASPNET.Business.Implemenatations;
//Repository
using RestWithASPNET.Repository;
using RestWithASPNET.Repository.Implemenatations;

namespace RestWithASPNET
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
            services.AddControllers();

            var connection = Configuration["MySQLConnection:MySQLConnectionString"];

            services.AddDbContext<MySqlContext>(options => options.UseMySql(
                connection, 
                new MySqlServerVersion(new System.Version())));
            services.AddApiVersioning();
               
                //Inje��o de dependencia
            services.AddScoped<IPersonBusiness, PersonBusinessImp>();
            services.AddScoped<IPersonRepository,PersonRepositoryImp>();
            
            
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
