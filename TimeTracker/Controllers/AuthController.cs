using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration
    ) : ControllerBase
{
    private const string RefreshTokenName = "RefreshToken";
    private const string LoginProvider = "TimeTrackerProvider";
    private readonly SymmetricSecurityKey _securityKey = new(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
    
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

    // [HttpPost("refresh")]
    // public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshRequest)
    // {
    //     if (!ValidateRefreshToken(refreshRequest.RefreshToken, out var _))
    //         return Unauthorized("Invalid refresh token.");
    //
    //     var user = await userManager.Users.FirstOrDefaultAsync(u =>
    //         userManager.GetAuthenticationTokenAsync(u, LoginProvider, RefreshTokenName).Result == refreshRequest.RefreshToken);
    //
    //     if (user == null)
    //         return Unauthorized("User associated with the refresh token was not found.");
    //     
    //     var storedRefreshToken = await userManager.GetAuthenticationTokenAsync(
    //         user,
    //         LoginProvider,
    //         RefreshTokenName
    //     );
    //
    //     if (storedRefreshToken == null || storedRefreshToken != refreshRequest.RefreshToken)
    //     {
    //         return Unauthorized("Invalid refresh token.");
    //     }
    //
    //     var newJwtToken = GenerateJwtToken(user);
    //     var newRefreshTokenValue = GenerateRefreshToken(user);
    //
    //     var setResult = await userManager.SetAuthenticationTokenAsync(
    //         user,
    //         LoginProvider,
    //         RefreshTokenName,
    //         newRefreshTokenValue
    //     );
    //
    //     if (!setResult.Succeeded)
    //     {
    //         return StatusCode(500, "Error updating refresh token.");
    //     }
    //
    //     return Ok(new
    //     {
    //         Token = newJwtToken,
    //         RefreshToken = newRefreshTokenValue
    //     });
    // }

    private string GenerateJwtToken(IdentityUser user)
    {
        var signingCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
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
        var signingCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
        var expireDays = Convert.ToDouble(configuration["Jwt:expireDays"]);
        var claims = new List<Claim> {
            new (JwtRegisteredClaimNames.Sub, user.Id)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(expireDays),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool ValidateRefreshToken(string token, out DateTime creationDate)
    {
        creationDate = DateTime.MinValue;

        var tokenHandler = new JwtSecurityTokenHandler();

        try {
            tokenHandler.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            if (validatedToken is JwtSecurityToken jwtToken && 
                jwtToken.Payload.TryGetValue(JwtRegisteredClaimNames.Iat, out var iatValue))
            {
                var iatSeconds = Convert.ToInt64(iatValue);
                creationDate = DateTimeOffset.FromUnixTimeSeconds(iatSeconds).UtcDateTime;
            }

            return true;
        } catch (SecurityTokenExpiredException) {
            Console.WriteLine("The token has expired.");
            return false;
        } catch (SecurityTokenInvalidSignatureException) {
            Console.WriteLine("The token signature is invalid.");
            return false;
        } catch (SecurityTokenException) {
            Console.WriteLine("There was an error with the token.");
            return false;
        } catch (ArgumentException) {
            Console.WriteLine("The token format is invalid.");
            return false;
        } catch (Exception ex) {
            Console.WriteLine($"An unknown error occurred: {ex.Message}");
            return false;
        }
    }
}
