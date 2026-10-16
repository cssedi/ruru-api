using ruru_api.DTOs.Auth;
using ruru_api.src.model;

namespace ruru_api.src.repositories.AuthRepository;

public interface IAuthRepository
{
    public Task<User?> CreateUser(RegisterUserRequestBody requestBody);
    public Task<User> SignIn(SignInRequestBody user);
    //public Task<User> GetUser(string username);
    //public Task<User> GetUserByEmail(string email);
    
}