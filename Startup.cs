using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TrackingAPI.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TrackingAPI.Repository;
using TrackingAPI.IRepository;
using TrackingAPI.Exceptions;
using Microsoft.Extensions.Logging;
using System.IO;
using NLog.Extensions.Logging;
using NLog;

namespace TrackingAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
           
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;


        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(o => o.AddPolicy("Cors", builder =>
            //{
            //    builder.WithOrigins("http://localhost:4200")
            //           .AllowAnyMethod()
            //           .AllowAnyHeader()
            //           .AllowCredentials();
            //}));
            services.AddCors();
            services.AddDbContext<DataContext>(cfg => {//AddDbContext does  DI
                cfg.UseSqlServer(Configuration.GetConnectionString("cn"));
            });
            services.AddTransient<IShipmentTracking,RShipmentTracking>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<CustomExceptionMiddleware>();
           
            //else
            //{
            //    app.UseHsts();
            //}
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
