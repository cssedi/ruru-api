using ruru_api.DTOs.Auth;
using ruru_api.src.model;

namespace ruru_api.services.AuthService;

public interface IAuthService
{
    public Task<RegisterUserResponseBody> CreateUserAsync(RegisterUserRequestBody requestBody);
}