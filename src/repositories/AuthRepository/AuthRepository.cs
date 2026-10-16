using Microsoft.AspNetCore.Identity;
using ruru_api.DTOs.Auth;
using ruru_api.services.AuthService;
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

    public async Task<User?> CreateUser(RegisterUserRequestBody requestBody)
    {
        User user = new User()
        {
            Name = requestBody.FirstName,
            Surname = requestBody.LastName,
            EmailAddress = requestBody.Email,
        };

        string userRole = nameof(User);

        try
        {
            var userResult = await _userManager.CreateAsync(user, requestBody.Password);
            var roleResult = await _userManager.AddToRoleAsync(user, userRole);
            if (userResult.Succeeded && roleResult.Succeeded)
            {
                // sign in user after registration
                await _signInManager.SignInAsync(user, false);
                return user;
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            
        }

        return user;
    }

    public async Task<User> SignIn(SignInRequestBody requestBody)
    {
        var user = await _userManager.FindByEmailAsync(requestBody.Email);
        
        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, requestBody.Password, false, false);
            if (result.Succeeded)
            {
                return user;
            }
        }

        return user;
    }
}