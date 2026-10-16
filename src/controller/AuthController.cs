using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ruru_api.DTOs.Auth;
using ruru_api.services.AuthService;
using ruru_api.services.WorkoutService;
using ruru_api.src.DTOs.Workouts;

namespace ruru_api.src.controller
{ 
    
     [Route("api/[controller]")]
     [ApiController]
     public class AuthController : ControllerBase
     {
         private readonly IAuthService _authService;
     
         public AuthController(IAuthService authService)
         {
             _authService = authService;
         }
         
         [HttpPost("register")]
         public async Task<IActionResult> Register(RegisterUserRequestBody dto)
         {
             var response = await _authService.CreateUserAsync(dto);

             if (response.Token == null)
             {
                 return BadRequest(new { response.Message });
             }
                
             return Ok(new {response});
         }
     }
}


