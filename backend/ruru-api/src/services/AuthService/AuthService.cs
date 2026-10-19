using Microsoft.AspNetCore.Identity;
using ruru_api.DTOs.Auth;
using ruru_api.src.common;
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

    public async Task<Result<UserSignInResponseBody>> CreateUserAsync(RegisterUserRequestBody requestBody)
    {
        if (requestBody.Password != requestBody.ConfirmPassword)
        {
            return Result<UserSignInResponseBody>.Failure("Passwords do not match.");
        }

        var result = await _authRepository.CreateUser(requestBody);
        if (!result.Succeeded)
        {
            return Result<UserSignInResponseBody>.Failure(result.Errors);
        }

        return Result<UserSignInResponseBody>.Success(new UserSignInResponseBody
        {
            Message = "Registration Successful!",
            Token = await GenerateJwtToken(result.Value!)
        });
    }

    public async Task<UserSignInResponseBody> SignInAsync(SignInRequestBody requestBody)
    {
        var user = await _authRepository.GetUser(requestBody.Email);
        
        if (user == null)
        {
            return new UserSignInResponseBody()
            {
                Message = "This Email address is not in our system, Would you like to create a new account?"
            };
        }
        
        var response = await _signInManager.PasswordSignInAsync(requestBody.Email, requestBody.Password, false, false);

        if (!response.Succeeded)
        {
            return new UserSignInResponseBody()
            {
                Message = "This email is not registered on the system. Would you like to create a new account?",
            };
        }
        return new UserSignInResponseBody()
        {
            Message = "User successfully signed in",
            Token = await GenerateJwtToken(user)
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
            new Claim("FullName", (user.FirstName + user.LastName) ?? string.Empty)
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