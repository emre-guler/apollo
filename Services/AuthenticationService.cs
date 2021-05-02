using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Apollo.Services
{
    public class AuthenticationService 
    {
        private string secureKey = "eebfb49f5b5b891f1ee6dfb79661a80f";
        public string GenerateToken(int ID)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);
            var payload = new JwtPayload(ID.ToString(), null, null, null, DateTime.Today.AddDays(1));
            var securityToken = new JwtSecurityToken(header, payload);
            
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}