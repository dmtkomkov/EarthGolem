using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Interfaces;

namespace TimeTracker.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration,
        ITokenService tokenService
    ) : ControllerBase
{
    private const string RefreshTokenName = "RefreshToken";
    private const string LoginProvider = "TimeTrackerProvider";
    
    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class TokensDto
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

        return await GenerateTokens(user);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokensDto refreshRequest)
    {
        if (!tokenService.ValidateRefreshToken(refreshRequest.Token, false, out var token) || token == null)
            return Unauthorized("Invalid token.");
        
        if (!tokenService.ValidateRefreshToken(refreshRequest.RefreshToken, true, out var refreshToken) || refreshToken == null)
            return Unauthorized("Invalid refresh token.");
    
        var userId = refreshToken.Claims
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

        return await GenerateTokens(user);
    }

    private async Task<IActionResult> GenerateTokens(IdentityUser user) {
        var expireMinutes = Convert.ToDouble(configuration["Jwt:ExpireMinutes"]);
        var expireDays = Convert.ToDouble(configuration["Jwt:ExpireDays"]);
        
        var newToken = tokenService.GenerateJwtToken(user.Id, DateTime.UtcNow.AddMinutes(expireMinutes));
        var newRefreshToken = tokenService.GenerateJwtToken(user.Id, DateTime.UtcNow.AddDays(expireDays));
    
        var setResult = await userManager.SetAuthenticationTokenAsync(
            user,
            LoginProvider,
            RefreshTokenName,
            newRefreshToken
        );
    
        if (!setResult.Succeeded)
        {
            return StatusCode(500, "Error updating refresh token.");
        }
    
        return Ok(new
        {
            Token = newToken,
            RefreshToken = newRefreshToken
        });
    }
}
