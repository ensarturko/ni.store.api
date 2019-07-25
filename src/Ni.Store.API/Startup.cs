using Ni.Store.Api.Data;
using Ni.Store.Api.Filters;
using Ni.Store.Api.Services;
using Ni.Store.Api.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ni.Store.Api.Data.Repositories;
using Ni.Store.Api.Data.Repositories.Implementations;
using Ni.Store.Api.Middleware;

namespace Ni.Store.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<IStoreRepository, StoreRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Ni.Store.Api", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
                c.DescribeStringEnumsInCamelCase();
            });

            services.AddDbContext<StoreDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("StoreConnectionString"));
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddMvcOptions(options => {
                    options.Filters.Add<GlobalExceptionFilter>();
                })
                .AddJsonOptions(options => {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                });

            services.AddHealthChecks().AddCheck<HealthCheckService>("default");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<LoggingMiddleware>();
            app.UseCors(options => options.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());

            app.UseSwagger();

            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("v1/swagger.json", "Ni.Store.Api");
            });

            app.UseHttpsRedirection();
            app.UseMvcWithDefaultRoute();
            app.UseHealthChecks("/healthcheck");
        }
    }
}