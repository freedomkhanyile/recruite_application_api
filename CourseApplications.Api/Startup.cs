using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CourseApplications.Api
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

            // Asuming that this service will be deployed in a cluster such as Kubernetes clusters
            // We add the health check so that a polling can be done against our Api and ensures that the 
            // Api is up and is able to self heal if its not available or stale.
            services.AddHealthChecks();

            // Swagger Documentation for external developers who will be usinng our Api service 
            // This can be new recruiting websites.

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0.0.1", new OpenApiInfo { Title = "CourseApplications.Api", Version = "v0.0.1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v0.0.1/swagger.json", "CourseApplications.Api v0.0.1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHealthChecks("/health", new HealthCheckOptions { ResponseWriter = JsonResponseWriter });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // Instead of the default text/plain response of Healthy,
        // we send an application/json response of {"status":"Healthy"}

        private async Task JsonResponseWriter(HttpContext httpContext, HealthReport report)
        {
            httpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(httpContext.Response.Body, new { Status = report.Status.ToString() },
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }


    }
}
