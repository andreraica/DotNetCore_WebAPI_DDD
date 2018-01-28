// Copyright (c) Nate Barbettini. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Infrastructure.CrossCutting.Configuration;
using Infrastructure.CrossCutting.Helper;
using System.Net;
using System.Collections.Generic;
using Application.Interface.Application;
using System.Linq;

namespace TokenProvider
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IParticipantApplication _participantApplication;

        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options,
            TokenValidationParameters tokenValidationParameters,
            ILoggerFactory loggerFactory,
            IParticipantApplication participantApplication)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<TokenProviderMiddleware>();
            _tokenValidationParameters = tokenValidationParameters;
            _participantApplication = participantApplication;

            _options = options.Value;
            ThrowIfInvalidOptions(_options);

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        public Task Invoke(HttpContext context)
        {
            bool isCreate = context.Request.Path.Equals(_options.Path, StringComparison.Ordinal);

            if (!isCreate)
            {
                if (context.Request.Cookies.TryGetValue("access_token_httponly", out string token))
                {
                    context.Request.Headers.Remove("Authorization");
                    context.Request.Headers.Append("Authorization", "Bearer " + token);
                }

                if (!context.Request.Headers.ContainsKey("Authorization"))
                {
                    if (!context.Request.Path.Value.Contains("swagger"))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return context.Response.WriteAsync("Authorização Negada - Header Authorization nao encontrado");
                    }
                }

                return _next(context);
            }

            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST")
               || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return context.Response.WriteAsync("Authorization Inválido / Envio de FormContent deve ser via Verbo POST");
            }

            //_logger.LogInformation($"Handling request for {(isCreate ? "create token": "refresh token")}: " + context.Request.Path);

            return GenerateToken(context);

        }

        private async Task GenerateToken(HttpContext context)
        {
            var grantType = context.Request.Form["grant_type"];
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];
            string key = context.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(key))
            {
                int firstSpace = key.IndexOf(" ");
                key = key.Substring(firstSpace + 1);
            }

            if (grantType != GrantType.Basic && grantType != GrantType.Password)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid grantType.");
                return;
            }

            var claims = new List<Claim>();

            if (key != Configuration.TokenKey && grantType != GrantType.Password)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Authorization basic invalid value.");
                return;
            }


            if (grantType == GrantType.Password)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Não é possivel receber um token por password");
                return;
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds
            };

            // Serialize and return the response
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _serializerSettings));
        }

        private async Task WriteTokenResponse(HttpContext context, JwtSecurityToken jwt)
        {
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds
            };

            // Serialize and return the response
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _serializerSettings));
        }

        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            if (string.IsNullOrEmpty(options.Path))
                throw new ArgumentNullException(nameof(TokenProviderOptions.Path));

            if (string.IsNullOrEmpty(options.Issuer))
                throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));

            if (string.IsNullOrEmpty(options.Audience))
                throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));

            if (options.Expiration == TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenProviderOptions.Expiration));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));

            if (options.NonceGenerator == null)
                throw new ArgumentNullException(nameof(TokenProviderOptions.NonceGenerator));
        }

        public static long ToUnixEpochDate(DateTime date) => new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds();
    }

    public static class GrantType
    {
        public static string Basic { get; set; } = "basic";
        public static string Password { get; set; } = "password";
    }
}
