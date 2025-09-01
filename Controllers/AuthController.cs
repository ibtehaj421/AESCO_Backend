using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{


    //register
    [HttpPost("register/user")]
    public async Task<string> Register([FromBody] CreateUserDto request) {
        if (request == null)
        {
            return "Invalid request data.";
        }
        return await Task.FromResult("User registered successfully for now.");
    }

}