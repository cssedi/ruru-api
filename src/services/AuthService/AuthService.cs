using Microsoft.AspNetCore.Identity;
using ruru_api.src.model;

namespace ruru_api.services.AuthService;

public class AuthService : IAuthService
{
    private readonly RuruDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthService(RuruDbContext context)
    {
        _context = context;
    }
    
    
    
}