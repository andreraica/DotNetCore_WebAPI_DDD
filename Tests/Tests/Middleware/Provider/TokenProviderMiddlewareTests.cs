using Application.Interface.Application;
using Infrastructure.CrossCutting.Configuration;
using Infrastructure.CrossCutting.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tests.Infrastructure.Stubs;
using TokenProvider;
using Xunit;

namespace Tests
{
    public class TokenProviderMiddlewareTests
    {
        //private readonly TokenProviderOptions _options;
        private readonly TokenValidationParameters _tokenValidationParameters;
        //private readonly ILogger _logger;
        //private readonly JsonSerializerSettings _serializerSettings;
        private readonly JWTTokenDecrypt _jwtTokenDecrypt;
        private readonly Mock<IParticipantApplication> _participantApplication;

        private readonly Mock<ILoggerFactory> _loggerFactory;

        private TokenProviderMiddleware _tokenProviderMiddleware;

        public TokenProviderMiddlewareTests()
        {
            _jwtTokenDecrypt = new JWTTokenDecrypt();
            //_loggerFactory = new Mock<ILoggerFactory>();
            _participantApplication = new Mock<IParticipantApplication>();

            string secretKey = "d71a5f83-8132-4c50-acd0-96ed6ebbf61d";
            
            var signingKey = new SymmetricSecurityKey(ASCIIEncoding.ASCII.GetBytes(secretKey));

            var tokenProviderOptions = new TokenProviderOptions
            {
                Path = "/token",
                Audience = "Audience",
                Issuer = "Issuer",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                ValidateIssuer = true,
                ValidIssuer = "Issuer",

                ValidateAudience = true,
                ValidAudience = "Audience",

                ValidateLifetime = true,
                
                ClockSkew = TimeSpan.Zero
            };

            Configuration.TokenKey = "Q2FpeGFNYWlzVmFudGFnZW5zTFRN";

            Configuration.Expiration = 15;

            _participantApplication.Setup(x => x.GetByCpf("12345678900")).Returns(ParticipantStub.Participante());

            _tokenProviderMiddleware = new TokenProviderMiddleware(null, Options.Create(tokenProviderOptions),
                tokenValidationParameters, _loggerFactory.Object, _participantApplication.Object);
            
        }

        [Fact]
        public async Task Deve_Gerar_Token_Tipo_Password_Ok()
        {
            var paramsForm = new Dictionary<String, StringValues>();
            paramsForm.Add("grant_type", "password");
            paramsForm.Add("username", "12345678900");
            paramsForm.Add("password", "123@321");

            var formCollection = new FormCollection(paramsForm);

            var context = new DefaultHttpContext();
            context.Request.Method = "POST";
            context.Request.Headers["Authorization"] = $"Basic {Configuration.TokenKey}";
            context.Request.Path = new PathString("/token");
            context.Request.Form = formCollection;

            await _tokenProviderMiddleware.Invoke(context);

            Assert.True(context.Response.StatusCode == (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task Nao_Deve_Gerar_Token_Tipo_Password_Com_Usuario_Inexistente()
        {
            var paramsForm = new Dictionary<String, StringValues>();
            paramsForm.Add("grant_type", "password");
            paramsForm.Add("username", "00000000000");
            paramsForm.Add("password", "123@321");

            var formCollection = new FormCollection(paramsForm);

            var context = new DefaultHttpContext();
            context.Request.Method = "POST";
            context.Request.Headers["Authorization"] = $"Basic {Configuration.TokenKey}";
            context.Request.Path = new PathString("/token");
            context.Request.Form = formCollection;

            await _tokenProviderMiddleware.Invoke(context);

            Assert.True(context.Response.StatusCode == (int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public void Deve_Gerar_Token_Tipo_Password_Com_Username()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6IjU0MTQzODk3NjQ0IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYmFua2luZyIsIm5iZiI6MTQ5OTk1MTUxOCwiZXhwIjoxNDk5OTUzMzE4LCJpc3MiOiJJc3N1ZXIiLCJhdWQiOiJBdWRpZW5jZSJ9.CuwW2vfk5k1f-RlpviehZkONLLzN0nJNyqo5qGxF83A";
            string username =_jwtTokenDecrypt.Get(token, "username");
            Assert.NotNull(username);
        }
    }
    
}