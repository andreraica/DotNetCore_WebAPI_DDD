using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.CrossCutting.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using TokenProvider;

namespace Api
{
    public partial class Startup
    {
        private static readonly string secretKey = "d71a5f83-8132-4c50-acd0-96ed6ebbf61d";

        private void ConfigureAuth(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseSimpleTokenProvider(new TokenProviderOptions
            {
                Path = "/token",
                Audience = "Audience",
                Issuer = "Issuer",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)//,
                //IdentityResolver = GetIdentity
            }, tokenValidationParameters);

            //app.UseJwtBearerAuthentication(new JwtBearerOptions
            //{
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    TokenValidationParameters = tokenValidationParameters
            //});

            app.UseAuthentication();


            // app.UseCookieAuthentication(new CookieAuthenticationOptions
            // {
            //     CookieDomain = Configuration.CookieDomain,
            //     AutomaticAuthenticate = true,
            //     AutomaticChallenge = true,
            //     AuthenticationScheme = "Cookies",  
            //     LoginPath = new PathString("/token"),               
            //     CookieSecure = env.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always,  //em produção, deve ficar: CookieSecurePolicy.Always 
            //     TicketDataFormat = new CustomJwtDataFormat(
            //         SecurityAlgorithms.HmacSha256,
            //         tokenValidationParameters),
            //     CookieHttpOnly = false,                    
            //     CookieName = "access_token",
            //     CookiePath = "/",                
            //     ExpireTimeSpan = TimeSpan.FromHours(1)    
            // });
        }

        // private Task<ClaimsIdentity> GetIdentity(string username, string password)
        // {
        //     // Don't do this in production, obviously!
        //     //if (username == "TEST" && password == "TEST123")
        //     //{
        //         return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));
        //     //}

        //     // Credentials are invalid, or account doesn't exist
        //     //return Task.FromResult<ClaimsIdentity>(null);
        // }
    }
}
