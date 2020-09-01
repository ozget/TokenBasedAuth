using IdentityTokenBasedAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace IdentityTokenBasedAuth.Security.Token
{
    public class TokenHandler : ITokenHandler
    {
        private readonly CustomTokenOptions customTokenOptions;

        public TokenHandler(IOptions<CustomTokenOptions> customTokenOptions)
        {
            this.customTokenOptions = customTokenOptions.Value;
        }
        public AccesToken CreateAccessToken(ApplicationUser user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(customTokenOptions.AccessTokenExpiration);
            var securityKey = SignHandler.GetSecurityKey(customTokenOptions.SecuntyKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: customTokenOptions.Issuer,
                audience: customTokenOptions.Audience,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaim(user),
                signingCredentials: signingCredentials
                );

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            AccesToken accesToken = new AccesToken();
            accesToken.Token = token;
            accesToken.RefreshToken = CreateRefreshToken();
            accesToken.Expiration = accessTokenExpiration;
            return accesToken;
        }

        public void RevokeRefreshToken(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];
            using(var mg = RandomNumberGenerator.Create())
            {
                mg.GetBytes(numberByte);
                return Convert.ToBase64String(numberByte);
            }
        }
        private IEnumerable<Claim> GetClaim(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.Email,user.Email),
               new Claim(ClaimTypes.Name,$"{user.UserName}"),
               new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
               };
            return claims;

        }
    }

}

