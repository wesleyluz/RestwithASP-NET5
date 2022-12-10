using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
//Model
using RestWithASPNET.Model.Context;
//Business
using RestWithASPNET.Business;
using RestWithASPNET.Business.Implemenatations;
//Repository
using RestWithASPNET.Repository;
using RestWithASPNET.Repository.Generic;
using Microsoft.Net.Http.Headers;
using RestWithASPNET.Hypermedia.Filters;
using RestWithASPNET.Hypermedia.Enricher;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Rewrite;
using RestWithASPNET.Services;
using RestWithASPNET.Services.Implementations;
using RestWithASPNET.Repository.UserRep;
using RestWithASPNET.Configurations;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace RestWithASPNET
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var tokenConfigurations = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                    Configuration.GetSection("TokenConfigurations")
                )
            .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenConfigurations.Issuer,
                    ValidAudience = tokenConfigurations.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer",new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));
           
            services.AddControllers();

            var connection = Configuration["MySQLConnection:MySQLConnectionString"]; 
            
            if (Environment.IsDevelopment())
            {
                MigrateDataBase(connection);
            }


            services.AddDbContext<MySqlContext>(options => options.UseMySql(
                connection, 
                new MySqlServerVersion(new System.Version())));

            //
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;

                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));

            })
            .AddXmlSerializerFormatters();

            var filterOptions = new HyperMediaFilterOptions();
            filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
            filterOptions.ContentResponseEnricherList.Add(new BookEnricher());
            services.AddSingleton(filterOptions);
            
           
            // Versioning
            services.AddApiVersioning();
            // Swagger
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Rest API",
                        Version = "v1",
                        Description = "Api development in progress",
                        Contact = new OpenApiContact 
                        {
                            Name = "Wesley Luz",
                            Url = new Uri("https://github.com/wesleyluz")
                        }
                    });
            });
               
            //Injeção de dependencia
                // Person
            services.AddScoped<IPersonBusiness, PersonBusinessImp>();
            services.AddScoped<IPersonRepository, PersonRepository>();
                // Book
            services.AddScoped<IBookBusiness, BookBusinessImp>();
            
                //Validation Injections
            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped<ILoginBusiness, LoginBusiness>()
                .AddScoped<IUserRepository, UserRepository>();

                //  Generic repository
            services.AddScoped(typeof(IRepository<>),typeof(GenericRepository<>));  
            
            
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
            // depois de UseHttp, routing antes de endpoints
            app.UseCors();

            app.UseSwagger();
            
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rest Api - v1");
            });

            var option = new RewriteOptions();
            option.AddRedirect("^$","swagger");
            app.UseRewriter(option); 


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}"); versão antiga
                endpoints.MapControllerRoute("DefaultApi", "{controller=values}/v{version=apiVersion}/{id?}");
                endpoints.MapRazorPages();
            });
        }
        private void MigrateDataBase(string connection)
        {
            try
            {
                var evolveConection = new MySql.Data.MySqlClient.MySqlConnection(connection);
                var evolve = new Evolve.Evolve(evolveConection, msg => Log.Information(msg))
                {
                    Locations = new List<string> { "db/migrations", "db/dataset" },
                    IsEraseDisabled = true,
                };
                evolve.Migrate();

            }
            catch (Exception ex)
            {
                Log.Error("Migration data base falied", ex);
                throw;
            }
        }
    }
}
