using Microsoft.AspNetCore.Mvc;
using ASCO.Services;
[ApiController]
[Route("role")]
public class RoleController : ControllerBase
{
    private readonly RoleService _roleService;

    public RoleController(RoleService roleService)
    {
        _roleService = roleService;
    }

    // [HttpPost("create")]
    // public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto request)
    // {


    // }

    //placeholder dummy for initial roles
    [HttpPost("master")]
    public async Task<string> CreateMasterRoles()
    {
        return await _roleService.CreateMasterRoles();
    }
    
}