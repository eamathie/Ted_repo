using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace tedMovieApp.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager; 
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // default role: User
        await _userManager.AddToRoleAsync(user, "User");

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email); 
        if (user == null) 
            return Unauthorized(); 
        
        var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordValid)
            return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user); 
        var token = GenerateJwtToken(user, roles); 
        
        return Ok(new { token });
    }

    private string GenerateJwtToken(IdentityUser user, IList<string> roles)
    {

        var keyString = Environment.GetEnvironmentVariable("JWT_KEY"); 
        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"); 
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString)); 
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Sub, user.Id), 
            new(JwtRegisteredClaimNames.Email, user.Email), 
            new(ClaimTypes.Name, user.UserName)
        }; 
        
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r))); 
        var token = new JwtSecurityToken( 
            issuer: issuer, 
            audience: audience, 
            claims: claims, 
            expires: DateTime.UtcNow.AddHours(1), 
            signingCredentials: creds); 
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    } 
    
    public record LoginDto(string Email, string Password);
    public record RegisterDto(string Email, string Password);
}