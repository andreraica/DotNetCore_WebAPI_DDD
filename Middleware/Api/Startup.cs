using Infrastructure.CrossCutting.Configuration;
using Infrastructure.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Middleware.Api.Filters;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Buffers;
using System.Text;

namespace Api
{
    public partial class Startup
    {
        private IConfigurationRoot _configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
                //.AddApplicationInsightsSettings();
            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ExcetionFilter());
                options.Filters.Add(new LogsRequestsAttribute());
                options.OutputFormatters.Clear();
                options.OutputFormatters.Add(new JsonOutputFormatter(new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                }, ArrayPool<char>.Shared));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Core API", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
                c.DocumentFilter<AuthTokenOperation>();
            });

            Configuration.CookieDomain = _configuration["CookieDomain"] ?? string.Empty;
            Configuration.ConnectionString = _configuration.GetConnectionString("SqlServerProvider");
            Configuration.TokenKey = _configuration.GetValue<string>("TokenKey");
            Configuration.Expiration = 15;

            RegisterServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Debug);
            loggerFactory.AddDebug();

            //if (env.IsDevelopment())
            //{
            app.UseCors(
                builder => builder.AllowAnyOrigin() //.WithOrigins("http://localhost:8080", "http://localhost:5000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );

            // Shows UseCors with CorsPolicyBuilder.
            //app.UseDeveloperExceptionPage();
            //}//https://docs.microsoft.com/en-us/aspnet/core/security/cors

            ConfigureAuth(app, env);

            //app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            //app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core API - V1"); });
        }

        private static void RegisterServices(IServiceCollection services)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "Issuer",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "Audience",

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                options.Authority = "http://localhost:30940/";
                options.Audience = "resource-server";
                options.RequireHttpsMetadata = false;
            });

            SimpleInjectorBootStrapper.RegisterServices(services);
        }
    }
}