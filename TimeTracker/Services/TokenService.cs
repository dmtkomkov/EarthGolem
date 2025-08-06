using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TimeTracker.Interfaces;

namespace TimeTracker.Services;

public class TokenService(
    ILogger<TokenService> logger,
    IConfiguration configuration
) : ITokenService {
    private readonly SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

    public string GenerateJwtToken(string userId, DateTime expires) {
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim> {
            new(JwtRegisteredClaimNames.Sub, userId)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateRefreshToken(string token, bool validateLifeTime, out JwtSecurityToken? jwtToken) {
        jwtToken = null;
        var tokenHandler = new JwtSecurityTokenHandler();

        try {
            tokenHandler.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = validateLifeTime,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            jwtToken = validatedToken as JwtSecurityToken;

            return true;
        }
        catch (SecurityTokenExpiredException) {
            logger.LogError("The token has expired");
            return false;
        }
        catch (SecurityTokenInvalidSignatureException) {
            logger.LogError("The token signature is invalid");
            return false;
        }
        catch (SecurityTokenException) {
            logger.LogError("There was an error with the token");
            return false;
        }
        catch (ArgumentException) {
            logger.LogError("The token format is invalid");
            return false;
        }
        catch (Exception ex) {
            logger.LogError("An unknown error occurred: {ExMessage}", ex.Message);
            return false;
        }
    }
}