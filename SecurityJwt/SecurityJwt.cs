using Domain.Constants;
using Domain.Entities;
using Domain.Models;
using Domain.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecurityJwt
{
    public class SecurityJwt : ISecurityJwt
    {
        private readonly SecuritySettings _securitySettings;
        public SecurityJwt(IOptions<SecuritySettings> securitySettings)
        {
            _securitySettings = securitySettings.Value;
        }

        public string GenerateToken(User user, int expiresMinutes)
        {
            var key = Encoding.ASCII.GetBytes(_securitySettings.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(SettingsConfig.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddMinutes(expiresMinutes).ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(expiresMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }

        public int ValidateToken(string token)
        {
            if (token == null)
                return 0;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_securitySettings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = _securitySettings.ValidateIssuer,
                    ValidateAudience = _securitySettings.ValidateAudience,
                    ValidIssuer = _securitySettings.ValidIssuer,
                    ValidAudience = _securitySettings.ValidAudience,
                    // set clockskew to zero so tokens expire exactly at token expiration time
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == SettingsConfig.Id).Value);
                return userId;
            }
            catch
            {
                return 0;
            }
        }

        #region Obtiene expiracion de token en minutos
        /// <summary>
        /// Obtiene expiracion de token en minutos
        /// </summary>
        /// <returns>expiracion en minutos</returns>
        public int GetMinuteJwtExpiration()
        {
            return _securitySettings.Expiration.HasValue ? _securitySettings.Expiration.Value : 0;
        }
        #endregion
    }
}
