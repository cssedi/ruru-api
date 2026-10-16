using Microsoft.AspNetCore.Identity;
using ruru_api.DTOs.Auth;
using ruru_api.src.model;
using ruru_api.src.repositories.AuthRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ruru_api.services.AuthService;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IConfiguration _configuration;
    
    public AuthService(IAuthRepository authRepository, UserManager<User> userManager, SignInManager<User> signInManager,IConfiguration configuration, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _authRepository = authRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _roleManager = roleManager;
    }

    public async Task<RegisterUserResponseBody> CreateUserAsync(RegisterUserRequestBody requestBody)
    {
        var response = await _authRepository.CreateUser(requestBody);

        if (response != null)
        {
            return new RegisterUserResponseBody()
            {
                Message = "Registration Successful!",
                Token = await GenerateJwtToken(response)
            };
        }
        return new RegisterUserResponseBody()
        {
            Message = "User creation failed. Check logs for details",
            Token = null
        };
    }
    
    
    private async Task<string> GenerateJwtToken(User? user)
    {
        if (user == null)
        {
            return "";
        }
        var jwtSettings = _configuration.GetSection("Api");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // unique token ID
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim("FullName", (user.Name + user.Surname) ?? string.Empty)
        };
        // Add all user roles as claims
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Authority"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"]!)),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    
    
}