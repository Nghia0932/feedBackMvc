using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace feedBackMvc.Helpers
{
    public class JwtTokenHelper
    {
        private readonly SymmetricSecurityKey _key;

        public JwtTokenHelper()
        {
            // Retrieve the SECRET_KEY from environment variables
            var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
            
            // Check if the secret key is not null or empty
            if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
            {
                throw new ArgumentException("The SECRET_KEY must be at least 32 characters long.");
            }

            _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        }

        public string GenerateAccessToken(int adminId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, adminId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool TryParseToken(string token, out int adminId)
        {
            adminId = 0;
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _key,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (principal.Identity is ClaimsIdentity identity)
                {
                    var idClaim = identity.FindFirst(ClaimTypes.Name);
                    if (idClaim != null && int.TryParse(idClaim.Value, out adminId))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // Handle token validation exceptions
            }

            return false;
        }
    }
}
