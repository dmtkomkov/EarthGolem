using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration,
        ILogger<AuthController> logger
    ) : ControllerBase
{
    private const string RefreshTokenName = "RefreshToken";
    private const string LoginProvider = "TimeTrackerProvider";
    private readonly SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
    
    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RefreshTokenDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await userManager.FindByNameAsync(loginDto.Username);
        if (user == null)
            return Unauthorized("Invalid username or password.");

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!signInResult.Succeeded)
            return Unauthorized("Invalid username or password.");

        var jwtToken = GenerateJwtToken(user);
        var refreshTokenValue = GenerateRefreshToken(user);
        
        var setResult = await userManager.SetAuthenticationTokenAsync(
            user,
            LoginProvider,
            RefreshTokenName,
            refreshTokenValue
        );

        if (!setResult.Succeeded)
        {
            return StatusCode(500, "Error saving refresh token.");
        }

        return Ok(new
        {
            Token = jwtToken,
            RefreshToken = refreshTokenValue
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshRequest)
    {
        if (!ValidateRefreshToken(refreshRequest.RefreshToken, out var jwtToken) || jwtToken == null)
            return Unauthorized("Invalid refresh token.");
    
        var userId = jwtToken.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        
        if (string.IsNullOrEmpty(userId))
            return Unauthorized($"sub {userId} not found in refresh token.");
        
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) {
            return Unauthorized("User associated with the refresh token was not found.");
        }

        var storedRefreshToken = await userManager.GetAuthenticationTokenAsync(
            user,
            LoginProvider,
            RefreshTokenName
        );
    
        if (storedRefreshToken == null || storedRefreshToken != refreshRequest.RefreshToken)
        {
            return Unauthorized("Invalid refresh token.");
        }
    
        var newJwtToken = GenerateJwtToken(user);
        var newRefreshTokenValue = GenerateRefreshToken(user);
    
        var setResult = await userManager.SetAuthenticationTokenAsync(
            user,
            LoginProvider,
            RefreshTokenName,
            newRefreshTokenValue
        );
    
        if (!setResult.Succeeded)
        {
            return StatusCode(500, "Error updating refresh token.");
        }
    
        return Ok(new
        {
            Token = newJwtToken,
            RefreshToken = newRefreshTokenValue
        });
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expireMinutes = Convert.ToDouble(configuration["Jwt:ExpireMinutes"]);
        var claims = new List<Claim> {
            new (JwtRegisteredClaimNames.Sub, user.Id)
        };
        
        var token = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken(IdentityUser user)
    {
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expireDays = Convert.ToDouble(configuration["Jwt:expireDays"]);
        var claims = new List<Claim> {
            new (JwtRegisteredClaimNames.Sub, user.Id)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool ValidateRefreshToken(string token, out JwtSecurityToken? jwtToken) {
        jwtToken = null;
        var tokenHandler = new JwtSecurityTokenHandler();

        try {
            tokenHandler.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            jwtToken = validatedToken as JwtSecurityToken;

            return true;
        } catch (SecurityTokenExpiredException) {
            logger.LogError("The token has expired");
            return false;
        } catch (SecurityTokenInvalidSignatureException) {
            logger.LogError("The token signature is invalid");
            return false;
        } catch (SecurityTokenException) {
            logger.LogError("There was an error with the token");
            return false;
        } catch (ArgumentException) {
            logger.LogError("The token format is invalid");
            return false;
        } catch (Exception ex) {
            logger.LogError("An unknown error occurred: {ExMessage}", ex.Message);
            return false;
        }
    }
}
