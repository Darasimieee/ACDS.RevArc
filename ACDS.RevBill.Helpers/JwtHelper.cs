using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ACDS.RevBill.Entities.Models;
using Microsoft.IdentityModel.Tokens;

namespace ACDS.RevBill.Helpers
{
    public class JwtHelper
    {
        public const string ISSUER = "https://localhost:7180/";
        public const string AUDIENCE = "https://localhost:7180/";
        public const string SECURITY_KEY = "tokenSecurityKey@1";

        public static TokenValidationParameters GetTokenParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = ISSUER,
                ValidAudience = AUDIENCE,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY))
            };
        }

        public static string GenerateJSONTenantToken(Tenancy tenancy)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> {
                new Claim("TenantId", tenancy.TenantId),
                new Claim("OrganisationId", tenancy.OrganisationId.ToString()),
                new Claim("ConnectionString", tenancy.ConnectionString),
            };

            var token = new JwtSecurityToken(ISSUER, AUDIENCE,
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}