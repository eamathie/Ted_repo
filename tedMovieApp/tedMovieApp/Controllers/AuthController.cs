using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;

    public AuthController(UserManager<IdentityUser> userManager, /*SignInManager<IdentityUser> signInManager,*/
        IConfiguration config)
    {
        _userManager = userManager; 
        _config = config;
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
        var jwtSection = _config.GetSection("Jwt"); 
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"])); 
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); 
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id), 
            new Claim(JwtRegisteredClaimNames.Email, user.Email), 
            new Claim(ClaimTypes.Name, user.UserName)
        }; 
        
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r))); 
        var token = new JwtSecurityToken( 
            issuer: jwtSection["ValidIssuer"], 
            audience: jwtSection["ValidAudience"], 
            claims: claims, expires: DateTime.UtcNow.AddHours(1), 
            signingCredentials: creds); 
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    } 
    
    public record LoginDto(string Email, string Password);
    public record RegisterDto(string Email, string Password);
}

