using Microsoft.AspNetCore.Identity;
using ruru_api.DTOs.Auth;
using ruru_api.services.AuthService;
using ruru_api.src.common;
using ruru_api.src.model;

namespace ruru_api.src.repositories.AuthRepository;

public class AuthRepository : IAuthRepository
{
    private readonly RuruDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public AuthRepository(RuruDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task<Result<User>> CreateUser(RegisterUserRequestBody requestBody)
    {
        var roleName = nameof(UserRole.User);

        // Check before creating the user: AddToRoleAsync throws (rather than failing) if the role is missing
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            return Result<User>.Failure($"Role '{roleName}' does not exist.");
        }

        var user = new User
        {
            UserName = requestBody.Email, // Identity requires a UserName
            Email = requestBody.Email,
            FirstName = requestBody.FirstName,
            LastName = requestBody.LastName,
        };

        // Fails with the password / unique-email rules configured in Program.cs
        var createResult = await _userManager.CreateAsync(user, requestBody.Password);
        if (!createResult.Succeeded)
        {
            return Result<User>.Failure(createResult.Errors.Select(e => e.Description));
        }

        var roleResult = await _userManager.AddToRoleAsync(user, roleName);
        if (!roleResult.Succeeded)
        {
            // Don't leave a user in the database without a role
            await _userManager.DeleteAsync(user);
            return Result<User>.Failure(roleResult.Errors.Select(e => e.Description));
        }

        return Result<User>.Success(user);
    }

    public async Task<User?> SignIn(SignInRequestBody requestBody)
    {
        var user = await _userManager.FindByEmailAsync(requestBody.Email);
        
        if (user  == null)
        {
            return null;
        }
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, requestBody.Password, false);
        if (!result.Succeeded)
        {
            return null;
        }
            
        return user;
    }

    public async Task<User?> GetUser(string email)
    {
        var response = await _userManager.FindByEmailAsync(email);
        if (response == null)
        {
            return null;
        }
        return response;
    }
    
    
}