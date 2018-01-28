using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.CrossCutting.Interfaces;

namespace Infrastructure.CrossCutting.Helper
{
    public class JWTTokenDecrypt:IJWTTokenDecrypt
    {
        public string Get(string token, string item)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            return tokenS.Claims.First(claim => claim.Type == item).Value.Replace(".","").Replace("-","");
        }
    }
}