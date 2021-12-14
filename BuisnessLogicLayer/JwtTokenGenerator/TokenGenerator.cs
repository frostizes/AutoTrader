using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogicLayer.JwtTokenGenerator;
using ContractEntities.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Util.Configuration;

namespace BuisnessLogicLayer.JwtTokenGenerator
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly ILogger<TokenGenerator> _tokenLogger;
        private readonly JwtConfig _jwtConfig;

        public TokenGenerator(IOptionsMonitor<JwtConfig> optionsMonitor, ILogger<TokenGenerator> tokenLogger)
        {
            _tokenLogger = tokenLogger;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        public string GenerateJwtToken(ApplicationUser applicationUser)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", applicationUser.Id),
                    new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
