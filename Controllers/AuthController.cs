using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using ASCO.Services;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{

    private readonly UserService _userService;
    public AuthController(UserService userService)
    {
        _userService = userService;
    }
    //register
    [HttpPost("register/user")]
    public async Task<string> Register([FromBody] CreateUserDto request) {
        if (request == null)
        {
            return "Invalid request data.";
        }
        return await _userService.RegisterUserAsync(request);
    }

}