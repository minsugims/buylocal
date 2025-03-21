using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BuyLocalApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BuyLocalApp.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager,
                            IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = new ApplicationUser 
        { 
            UserName = model.UserName, 
            //NormalizedUserName = model.UserName.Trim().ToUpper(),
            Email = model.Email, 
            NormalizedEmail = model.Email.ToUpper(),
            FullName = model.FullName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var userValid = await _userManager.CheckPasswordAsync(user, model.Password);
        if (user == null || !userValid)
            return Unauthorized(new { message = "Invalid credentials" });

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.UtcNow.AddHours(12),
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
