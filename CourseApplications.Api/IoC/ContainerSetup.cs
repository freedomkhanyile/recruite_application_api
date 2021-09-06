
using CourseApplications.Api.Security;
using CourseApplications.DAL.Context;
using CourseApplications.DAL.UnitOfWork;
using CourseApplications.Services;
using CourseApplications.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApplications.Api.IoC
{
    public static class ContainerSetup
    {
        public static void SetUp(IServiceCollection services)
        {
            ConfigureAuth(services);
            ConfigureCors(services);
            AddServices(services);
            AddUnitOfWork(services);
        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
        }

        private static void AddUnitOfWork(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork>(uow => new EFUnitOfWork(uow.GetRequiredService<CourseApplicationDbContext>()));
        }

        private static void ConfigureAuth(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ITokenBuilder, TokenBuilder>();
            services.AddScoped<ISecurityContext, SecurityContext>();
        }
    }
}
