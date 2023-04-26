using Domain.Entities;

namespace Domain.Security
{
    public interface ISecurityJwt
    {
        string GenerateToken(User user, int expiresMinutes);
        int GetMinuteJwtExpiration();
        int ValidateToken(string token);
    }
}
