using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Apollo.DTO;
using Apollo.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Apollo.Services
{
    public class AuthenticationService 
    {
        private string secureKey = "eebfb49f5b5b891f1ee6dfb79661a80f";
        public string GenerateToken(JwtClaimDTO data)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new []
            {
                new Claim(JwtRegisteredClaimNames.Sub, data.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, data.Name),
                new Claim(JwtRegisteredClaimNames.Sub, data.MailAddress),
                new Claim(JwtRegisteredClaimNames.Sub, data.UserType.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: secureKey,
                audience: secureKey,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }

        public JwtClaimDTO GetUserData(ClaimsIdentity identity)
        {
            try
            {
                var claims = identity.Claims.ToList();
                if(claims.Count() != 0)
                {
                    JwtClaimDTO jwtData = new()
                    {
                        Id = int.Parse(claims[0].Value),
                        Name = claims[1].Value,
                        MailAddress = claims[2].Value,
                        UserType =  (UserType) int.Parse(claims[3].Value)
                    };
                    return jwtData;
                }
                return null;
                
            }
            catch(Exception e)
            {
                Console.Write(e);
            }
            return  null;
        }
    }
}