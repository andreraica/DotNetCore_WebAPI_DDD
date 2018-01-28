using Application.Application;
using Application.Interface.Application;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Dapper;
using Domain.Interfaces.Services;
using Domain.Service.Services;
using Infrastructure.CrossCutting.Cache;
using Infrastructure.CrossCutting.Helper;
using Infrastructure.CrossCutting.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.Repositories.Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IoC
{
    public class SimpleInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // App 
            services.AddSingleton(typeof(IApplicationBase<>), typeof(ApplicationBase<>));
            services.AddSingleton<IParticipantApplication, ParticipantApplication>();

            // Service
            services.AddSingleton(typeof(IServiceBase<>), typeof(ServiceBase<>));
            services.AddSingleton<IParticipantService, ParticipantService>();
            services.AddSingleton<ICacheManager,CacheManager>();
            
            // Repositories
            services.AddSingleton(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddSingleton<IParticipantRepository, ParticipantRepository>();
            services.AddSingleton<IParticipantDapper,ParticipantDapper>();

            // CrossCutting
            services.AddSingleton<IHttpClientHelper, HttpClientHelper>();
            services.AddSingleton<IJWTTokenDecrypt,JWTTokenDecrypt>();
            
            services.AddScoped<AADbContext>();
        }
    }
}