using System.IdentityModel.Tokens.Jwt;

namespace TimeTracker.Interfaces;

public interface ITokenService {
    string GenerateJwtToken(string userId, DateTime expires);
    bool ValidateRefreshToken(string token, bool validateLifeTime, out JwtSecurityToken? jwtToken);
}